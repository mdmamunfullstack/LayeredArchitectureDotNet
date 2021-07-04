// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Data;

namespace LayeredArchitecture.DataAccess.Base
{
    /// <summary>
    /// Used to create a new <see cref="DbConnectionScope"/> by using the supplied <see cref="IDbConnectionFactory"/>.
    /// </summary>
    public class DbConnectionScopeFactory
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public DbConnectionScopeFactory(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public DbConnectionScope Create(bool join = true, bool suppress = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var connection = dbConnectionFactory.Create();

            if (connection.State != ConnectionState.Closed)
            {
                throw new Exception("The database connection factory returned a database connection in a non-closed state. This behavior is not allowed as the ambient database scope will maintain database connection state as required.");
            }

            return new DbConnectionScope(connection, join, suppress, isolationLevel);
        }
    }
}
