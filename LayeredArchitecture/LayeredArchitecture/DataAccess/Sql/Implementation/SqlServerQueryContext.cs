// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.Common;

namespace LayeredArchitecture.DataAccess.Sql
{
    public class SqlServerQueryContext : SqlQueryContext
    {
        public SqlServerQueryContext(ILoggerFactory loggerFactory, DbConnection connection, IEnumerable<IEntityTypeMap> mappings)
            : base(loggerFactory, connection, mappings) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options, DbConnection connection, ILoggerFactory loggerFactory)
        {
            options
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging()
                .UseSqlServer(connection);
        }
    }
}
