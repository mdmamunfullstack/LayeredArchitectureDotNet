// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.Common;

namespace LayeredArchitecture.DataAccess.Sql
{
    /// <summary>
    /// Implements a <see cref="DbContext"/> used for querying the database.
    /// </summary>
    public abstract class SqlQueryContext : DbContext
    {
        private readonly DbConnection connection;
        private readonly ILoggerFactory loggerFactory; 
        private readonly IEnumerable<IEntityTypeMap> mappings;

        /// <summary>
        /// Creates a new <see cref="DbContext"/> to query the database.
        /// </summary>
        /// <param name="loggerFactory">A Logger Factory to enable EF Core Logging facilities</param>
        /// <param name="connection">An opened <see cref="DbConnection"/> to enlist to</param>
        /// <param name="mappings">The <see cref="IEntityTypeMap"/> mappings for mapping query results</param>
        public SqlQueryContext(ILoggerFactory loggerFactory, DbConnection connection, IEnumerable<IEntityTypeMap> mappings)
        {
            this.connection = connection;
            this.mappings = mappings;
            this.loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            OnConfiguring(options, connection, loggerFactory);
        }

        protected abstract void OnConfiguring(DbContextOptionsBuilder options, DbConnection connection, ILoggerFactory loggerFactory);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var mapping in mappings)
            {
                mapping.Map(modelBuilder);
            }
        }
    }
}
