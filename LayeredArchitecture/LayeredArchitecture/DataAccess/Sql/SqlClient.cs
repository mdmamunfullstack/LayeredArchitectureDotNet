// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LayeredArchitecture.DataAccess.Sql
{
    /// <summary>
    /// Provides simplified Access to the <see cref="DbContext"/> for CRUD Operations and raw SQL Queries.
    /// </summary>
    public class SqlClient
    {
        /// <summary>
        /// A factory to generate a fresh DbContext for database queries.
        /// </summary>
        private readonly SqlQueryContextFactory sqlQueryContextFactory;

        /// <summary>
        /// Creates an <see cref="EntityFrameworkSqlClient"/> based on the given <see cref="IEntityTypeMap"/> mappings.
        /// </summary>
        /// <param name="sqlQueryContextFactory">A Factory to create a Query DbContext</param>
        public SqlClient(SqlQueryContextFactory sqlQueryContextFactory)
        {
            
            this.sqlQueryContextFactory = sqlQueryContextFactory;
        }

        /// <summary>
        /// SQL Query on the Database for an Entity Type.
        /// </summary>
        /// <typeparam name="TEntityType">Entity Type to Query for</typeparam>
        /// <param name="connection"><see cref="DbConnection"/> used to query</param>
        /// <param name="sql">SQL Command as <see cref="FormattableString"/> to prevent SQL Injection Attacks</param>
        /// <param name="config">Additional Configuration for Querying the data, such as including related data</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to cancel the Operation</param>
        /// <returns>SQL Query results</returns>
        public async Task<List<TEntityType>> SqlQuery<TEntityType>(DbConnection connection, DbTransaction transaction, FormattableString sql, Func<IQueryable<TEntityType>, IQueryable<TEntityType>> config = null, CancellationToken cancellationToken = default(CancellationToken))
            where TEntityType : class
        {
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                await PrepareDbContextAsync(context, transaction, cancellationToken);

                var queryable = context.Set<TEntityType>()
                    .FromSqlInterpolated(sql)
                    .AsNoTracking();

                if (config != null)
                {
                    queryable = config(queryable);
                }

                return await queryable
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs a LINQ Query for a given entity.
        /// </summary>
        /// <typeparam name="TEntityType">Type of the Entity</typeparam>
        /// <param name="connection">Current Connection</param>
        /// <param name="transaction">Current Transaction</param>
        /// <param name="query">The LINQ Query</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A Task</returns>
        public async Task<List<TEntityType>> QueryAsync<TEntityType>(DbConnection connection, DbTransaction transaction, Func<IQueryable<TEntityType>, IQueryable<TEntityType>> query, CancellationToken cancellationToken)
            where TEntityType : class
        {
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                await PrepareDbContextAsync(context, transaction, cancellationToken);

                var entityQueryable = context
                    .Set<TEntityType>()
                    .AsQueryable();

                if (query != null)
                {
                    entityQueryable = query(entityQueryable);
                }

                return await entityQueryable
                    .AsNoTracking()
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Executes Raw SQL Commands.
        /// </summary>
        /// <param name="connection">Current Connection</param>
        /// <param name="transaction">Current Transaction</param>
        /// <param name="sql">Formattable SQL Command</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A Task</returns>
        public async Task<int> ExecuteRawSqlInterpolatedAsync(DbConnection connection, DbTransaction transaction, FormattableString sql, CancellationToken cancellationToken)
        {
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                await PrepareDbContextAsync(context, transaction, cancellationToken);

                return await context.Database
                    .ExecuteSqlInterpolatedAsync(sql, cancellationToken)
                    .ConfigureAwait(false);
            }
        }


        /// <summary>
        /// Executes Raw SQL Commands.
        /// </summary>
        /// <param name="connection">Current Connection</param>
        /// <param name="transaction">Current Transaction</param>
        /// <param name="sql">Formattable SQL Command</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A Task</returns>
        public async Task<int> ExecuteRawSqlAsync(DbConnection connection, DbTransaction transaction, string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                await PrepareDbContextAsync(context, transaction, cancellationToken);

                return await context.Database
                    .ExecuteSqlRawAsync(sql, parameters, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Inserts a given Entity into the Database.
        /// </summary>
        /// <typeparam name="TEntityType">Type of the Entity</typeparam>
        /// <param name="connection">Current Connection</param>
        /// <param name="transaction">Current Transaction</param>
        /// <param name="entity">Entity to insert</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A Task</returns>
        public async Task InsertAsync<TEntityType>(DbConnection connection, DbTransaction transaction, TEntityType entity, CancellationToken cancellationToken = default(CancellationToken))
            where TEntityType : class
        {
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                await PrepareDbContextAsync(context, transaction, cancellationToken);

                context.Entry(entity).State = EntityState.Added;

                await context
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Inserts a Range of Entities to the Database.
        /// </summary>
        /// <typeparam name="TEntityType">Type of the Entity</typeparam>
        /// <param name="connection">Current Connection</param>
        /// <param name="transaction">Current Transaction</param>
        /// <param name="entities">Entities to insert</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A Task</returns>
        public async Task InsertRangeAsync<TEntityType>(DbConnection connection, DbTransaction transaction, IEnumerable<TEntityType> entities, CancellationToken cancellationToken = default(CancellationToken))
            where TEntityType : class
        {
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                await PrepareDbContextAsync(context, transaction, cancellationToken);

                foreach(var entity in entities)
                {
                    context.Entry(entity).State = EntityState.Added;
                }
                
                context
                    .Set<TEntityType>()
                    .AddRange(entities);

                await context
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Finds an entity with the given primary key values. If no entity is found, a null value is returned.
        /// </summary>
        /// <typeparam name="TEntityType">Type of the Entity</typeparam>
        /// <param name="connection">Current Connection</param>
        /// <param name="transaction">Current Transaction</param>
        /// <param name="keyValues">Primary Key Values</param>
        /// <returns>Entity or Null</returns>
        public async ValueTask<TEntityType> FindAsync<TEntityType>(DbConnection connection, DbTransaction transaction, object[] keyValues, CancellationToken cancellationToken = default(CancellationToken))
            where TEntityType : class
        {
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                await PrepareDbContextAsync(context, transaction, cancellationToken);

                return await context.Set<TEntityType>()
                    .FindAsync(keyValues, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Finds an entity with the given primary key values. If no entity is found, a null value is returned.
        /// </summary>
        /// <typeparam name="TEntityType">Type of the Entity</typeparam>
        /// <param name="connection">Current Connection</param>
        /// <param name="transaction">Current Transaction</param>
        /// <param name="keyValues">Primary Key Values</param>
        /// <returns>Entity or Null</returns>
        public async ValueTask<TEntityType> FindAsync<TEntityType>(DbConnection connection, DbTransaction transaction, params object[] keyValues)
            where TEntityType : class
        {
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                // TODO: Add CancellationToken
                await PrepareDbContextAsync(context, transaction, default(CancellationToken));

                return await context.Set<TEntityType>()
                    .FindAsync(keyValues)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Deletes an entity from the Database.
        /// </summary>
        /// <typeparam name="TEntityType">Type of the Entity</typeparam>
        /// <param name="connection">Current Connection</param>
        /// <param name="transaction">Current Transaction</param>
        /// <param name="entity">Entity to delete</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A Task</returns>
        public async Task DeleteAsync<TEntityType>(DbConnection connection, DbTransaction transaction, TEntityType entity, CancellationToken cancellationToken = default(CancellationToken))
            where TEntityType : class
        {
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                await PrepareDbContextAsync(context, transaction, cancellationToken);

                context.Entry(entity).State = EntityState.Deleted;

                await context
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Deletes a Range of Entities from the Database.
        /// </summary>
        /// <typeparam name="TEntityType">Type of the Entity</typeparam>
        /// <param name="connection">Current Connection</param>
        /// <param name="transaction">Current Transaction</param>
        /// <param name="entities">Entities to delete</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A Task</returns>
        public async Task DeleteRangeAsync<TEntityType>(DbConnection connection, DbTransaction transaction, IEnumerable<TEntityType> entities, CancellationToken cancellationToken = default(CancellationToken))
            where TEntityType : class
        {
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                await PrepareDbContextAsync(context, transaction, cancellationToken);

                foreach (var entity in entities)
                {
                    context.Entry(entity).State = EntityState.Deleted;
                }

                await context
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Updates an entity in the Database.
        /// </summary>
        /// <typeparam name="TEntityType">Type of the Entity</typeparam>
        /// <param name="connection">Current Connection</param>
        /// <param name="transaction">Current Transaction</param>
        /// <param name="entity">Entity to delete</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A Task</returns>
        public async Task UpdateAsync<TEntityType>(DbConnection connection, DbTransaction transaction, TEntityType entity, CancellationToken cancellationToken = default(CancellationToken))
            where TEntityType : class
        {
            
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                await PrepareDbContextAsync(context, transaction, cancellationToken);

                context.Entry(entity).State = EntityState.Modified;

                await context
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Updates a Range of Entities in the Database.
        /// </summary>
        /// <typeparam name="TEntityType">Type of the Entity</typeparam>
        /// <param name="connection">Current Connection</param>
        /// <param name="transaction">Current Transaction</param>
        /// <param name="entities">Entities to update</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A Task</returns>
        public async Task UpdateRangeAsync<TEntityType>(DbConnection connection, DbTransaction transaction, IEnumerable<TEntityType> entities, CancellationToken cancellationToken = default(CancellationToken))
            where TEntityType : class
        {
            
            using (var context = sqlQueryContextFactory.Create(connection))
            {
                await PrepareDbContextAsync(context, transaction, cancellationToken);

                foreach (var entity in entities)
                {
                    context.Entry(entity).State = EntityState.Modified;
                }

                await context
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }


        #region Context Setup

        /// <summary>
        /// Prepares the underlying <see cref="DbContext"/>, so Automatic Transactions are disabled on SaveChanges 
        /// and the current Transaction for the Connection is supplied. So we enlist in the local transaction.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/></param>
        /// <param name="transaction">The <see cref="DbTransaction"/> of the current <see cref="DbConnectionScope"/></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A Task</returns>
        private async Task PrepareDbContextAsync(SqlQueryContext context, DbTransaction transaction, CancellationToken cancellationToken)
        {
            if (transaction != null)
            {
                context.Database.AutoTransactionsEnabled = false;

                await context.Database
                    .UseTransactionAsync(transaction, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        #endregion
    }
}