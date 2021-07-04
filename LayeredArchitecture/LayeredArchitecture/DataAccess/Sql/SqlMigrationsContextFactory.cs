// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace LayeredArchitecture.DataAccess.Sql
{
    /// <summary>
    /// Enabling automatic migrations for all Entities mapped to a <see cref="SqlMigrationsContext"/>.
    /// </summary>
    public abstract class SqlMigrationsContextFactory : IDesignTimeDbContextFactory<SqlMigrationsContext>
    {
        public SqlMigrationsContext CreateDbContext(string[] args)
        {
            // Register all Dependencies:
            var services = new ServiceCollection();

            RegisterDependencies(services);

            // Build the ServiceProvider:
            var serviceProvider = services.BuildServiceProvider();

            // Get the Entity Mappings:
            var entityTypeMappings = serviceProvider.GetServices<IEntityTypeMap>();

            // Builder the Options:
            var builder = new DbContextOptionsBuilder<SqlMigrationsContext>();

            ConfigureDbContextOptions(builder);

            // Build the Context:
            return new SqlMigrationsContext(builder.Options, entityTypeMappings);
        }

        /// <summary>
        /// Configures the DbContext, for example the Database to use.
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void ConfigureDbContextOptions(DbContextOptionsBuilder<SqlMigrationsContext> builder);

        /// <summary>
        /// Registers the Dependencies needed to bootstrap the MigrationsContext.
        /// </summary>
        /// <param name="services"></param>
        protected abstract void RegisterDependencies(ServiceCollection services);
    }
}
