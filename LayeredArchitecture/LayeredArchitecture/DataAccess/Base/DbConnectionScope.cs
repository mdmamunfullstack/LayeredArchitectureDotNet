// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Base.Model;
using System;
using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LayeredArchitecture.DataAccess.Base
{
    /// <summary>
    /// Context Key to identify the current <see cref="DbConnectionScope"/> in the <see cref="AsyncLocalStorage"/>.
    /// </summary>
    internal sealed class ContextKey
    {
    }

    /// <summary>
    /// Stores the <see cref="DbConnectionScope"/> in a <see cref="ConditionalWeakTable{TKey, TValue}"/>, so the current scope flows with the async / await.
    /// </summary>
    internal static class AsyncLocalStorage
    {
        private static readonly AsyncLocal<ContextKey> CurrentContextKey = new AsyncLocal<ContextKey>();

        private static readonly ConditionalWeakTable<ContextKey, ImmutableStack<DbConnectionScope>> DbConnectionScopes = new ConditionalWeakTable<ContextKey, ImmutableStack<DbConnectionScope>>();

        public static void SaveStack(ImmutableStack<DbConnectionScope> stack)
        {
            var contextKey = CurrentContextKey.Value;

            if (contextKey == null)
            {
                throw new Exception("No Key found for Scope.");
            }

            if (DbConnectionScopes.TryGetValue(contextKey, out _))
            {
                DbConnectionScopes.Remove(contextKey);
            }

            DbConnectionScopes.Add(contextKey, stack);
        }

        public static ImmutableStack<DbConnectionScope> GetStack()
        {
            var contextKey = CurrentContextKey.Value;

            if (contextKey == null)
            {
                contextKey = new ContextKey();

                CurrentContextKey.Value = contextKey;
                DbConnectionScopes.Add(contextKey, ImmutableStack.Create<DbConnectionScope>());
            }

            bool keyFound = DbConnectionScopes.TryGetValue(contextKey, out var dbConnectionScopes);

            if (!keyFound)
            {
                throw new Exception("Stack not found for this DbConnectionScope");
            }

            return dbConnectionScopes;
        }
    }

    /// <summary>
    /// The DbConnectionScope uses the <see cref="AsyncLocalStorage"/> to ensure we use a single connection through the Lifetime of a method invoication.
    /// </summary>
    public class DbConnectionScope : IDisposable
    {
        protected DbConnectionScope Parent;
        protected DbConnection Connection;
        protected DbTransaction Transaction;
        protected IsolationLevel IsolationLevel;
        protected bool Suppress;

        public DbConnectionScope(DbConnection connection, bool join, bool suppress, IsolationLevel isolationLevel)
        {
            var currentStack = AsyncLocalStorage.GetStack();

            IsolationLevel = isolationLevel;
            Suppress = suppress;
            Connection = connection;

            if (join)
            {
                if (!currentStack.IsEmpty)
                {
                    var parent = currentStack.Peek();

                    Parent = parent;
                    Connection = parent.Connection;
                    Transaction = parent.Transaction;
                    Suppress = parent.Suppress;
                    IsolationLevel = parent.IsolationLevel;
                }
            }

            currentStack = currentStack.Push(this);

            AsyncLocalStorage.SaveStack(currentStack);
        }

        public ConnectionTransactionHolder GetConnection()
        {
            if (Parent == null && Connection.State != ConnectionState.Open)
            {
                Connection.Open();

                if (!Suppress)
                {
                    Transaction = Connection.BeginTransaction(IsolationLevel);
                }
            }

            // Has a parent, but their connection was never opened:
            if (Parent != null && Parent.Connection.State == ConnectionState.Closed)
            {
                Parent.Connection.Open();

                if (!Parent.Suppress)
                {
                    Parent.Transaction = Parent.Connection.BeginTransaction(Parent.IsolationLevel);
                }
            }

            // Opened the parent transaction, now inherit the transaction:
            if (Parent != null && Parent.Connection.State == ConnectionState.Open)
            {
                if (Parent.Transaction != null && Transaction == null)
                {
                    Transaction = Parent.Transaction;
                }
            }

            return new ConnectionTransactionHolder(Connection, Transaction);
        }

        public void Commit()
        {
            if (Parent != null)
            {
                return;
            }

            try
            {
                if (Transaction != null)
                {
                    Transaction.Commit();
                    Transaction.Dispose();
                    Transaction = null;
                }
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        public void Rollback()
        {
            if (Parent != null)
            {
                return;
            }

            try
            {
                if (Transaction != null)
                {
                    Transaction.Rollback();
                    Transaction.Dispose();
                    Transaction = null;
                }
            }
            catch (Exception)
            {
                if (Transaction != null && Transaction.Connection != null)
                {
                    Transaction.Dispose();
                    Transaction = null;
                }

                throw;
            }
        }

        public void Dispose()
        {
            var currentStack = AsyncLocalStorage.GetStack();

            if (currentStack.IsEmpty)
            {
                throw new Exception("Could not dispose scope because it does not exist in storage.");
            }

            var topItem = currentStack.Peek();

            if (this != topItem)
            {
                throw new InvalidOperationException("Could not dispose scope because it is not the active scope. This could occur because scope is being disposed out of order.");
            }

            currentStack = currentStack.Pop();

            AsyncLocalStorage.SaveStack(currentStack);

            if (Parent == null)
            {
                if (Transaction != null)
                {
                    Commit();
                }

                if (Connection != null)
                {
                    if (Connection.State == ConnectionState.Open)
                    {
                        Connection.Close();
                    }

                    Connection.Dispose();
                    Connection = null;
                }
            }

            GC.SuppressFinalize(this);
            GC.WaitForPendingFinalizers();
        }
    }
}
