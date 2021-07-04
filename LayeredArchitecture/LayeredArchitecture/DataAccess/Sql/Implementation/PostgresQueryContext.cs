// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace LayeredArchitecture.DataAccess.Sql
{
    public class PostgresQueryContext : SqlQueryContext
    {
        public PostgresQueryContext(ILoggerFactory loggerFactory, DbConnection connection, IEnumerable<IEntityTypeMap> mappings)
            : base(loggerFactory, connection, mappings) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options, DbConnection connection, ILoggerFactory loggerFactory)
        {
            options
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging()
                .UseNpgsql(connection);
        }
    }
}
