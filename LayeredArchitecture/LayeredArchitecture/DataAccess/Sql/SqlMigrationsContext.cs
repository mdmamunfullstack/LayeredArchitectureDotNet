// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LayeredArchitecture.DataAccess.Sql
{
    /// <summary>
    /// The Query DbContext expects an (opened) connection, so we cannot use it to create Migrations with the .NET tooling. Instead 
    /// of adding another constructor only used for migrations to the QueryContext, let's just add a completely separate context for 
    /// the migrations.
    /// 
    /// All we are adding for both are the <see cref="IEntityTypeMap"/> mappings anyway.
    /// </summary>
    public class SqlMigrationsContext : DbContext
    {
        private readonly IEnumerable<IEntityTypeMap> mappings;

        public SqlMigrationsContext(DbContextOptions<SqlMigrationsContext> options, IEnumerable<IEntityTypeMap> mappings)
            : base(options)
        {
            this.mappings = mappings;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var mapping in mappings)
            {
                mapping.Map(modelBuilder);
            }
        }
    }
}
