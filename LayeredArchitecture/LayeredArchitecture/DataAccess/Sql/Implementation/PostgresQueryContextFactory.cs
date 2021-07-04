// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.Common;

namespace LayeredArchitecture.DataAccess.Sql
{
    /// <summary>
    /// Generates a <see cref="SqlServerQueryContext"/>.
    /// </summary>
    public class PostgresQueryContextFactory : SqlQueryContextFactory
    {
        public PostgresQueryContextFactory(ILoggerFactory loggerFactory, IEnumerable<IEntityTypeMap> mappings)
            : base(loggerFactory, mappings) { }

        public override SqlQueryContext Create(DbConnection connection)
        {
            return new PostgresQueryContext(loggerFactory, connection, mappings);
        }
    }
}
