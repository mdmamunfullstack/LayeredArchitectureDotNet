// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.DataAccess.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LayeredArchitecture.Tests.DataAccess.Migrations
{

    public class CustomMigrationsContext : SqlMigrationsContextFactory
    {
        protected override void ConfigureDbContextOptions(DbContextOptionsBuilder<SqlMigrationsContext> builder)
        {
            builder
                .UseSqlServer(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=SampleDb;Integrated Security=SSPI;", 
                    b => b.MigrationsAssembly("LayeredArchitecture.Tests"));
        }

        protected override void RegisterDependencies(ServiceCollection services)
        {
            Bootstrapper.RegisterDependencies(services);
        }
    }
}
