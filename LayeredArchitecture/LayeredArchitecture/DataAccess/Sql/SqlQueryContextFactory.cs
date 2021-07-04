// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.Common;

namespace LayeredArchitecture.DataAccess.Sql
{
    /// <summary>
    /// Generates a <see cref="SqlServerQueryContext"/> to query the database.
    /// </summary>
    public abstract class SqlQueryContextFactory
    {
        protected readonly ILoggerFactory loggerFactory;
        protected readonly IEnumerable<IEntityTypeMap> mappings;

        public SqlQueryContextFactory(ILoggerFactory loggerFactory, IEnumerable<IEntityTypeMap> mappings)
        {
            this.loggerFactory = loggerFactory;
            this.mappings = mappings;
        }

        public abstract SqlQueryContext Create(DbConnection connection);
    }
}
