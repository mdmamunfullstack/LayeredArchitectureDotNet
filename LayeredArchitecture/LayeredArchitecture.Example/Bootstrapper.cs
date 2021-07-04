// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Identity;
using LayeredArchitecture.Base.Tenancy;
using LayeredArchitecture.Base.Tenancy.Model;
using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.DataAccess.Mapping;
using LayeredArchitecture.DataAccess.Sql;
using LayeredArchitecture.Example.App;
using LayeredArchitecture.Example.App.Implementation;
using LayeredArchitecture.Example.Business;
using LayeredArchitecture.Example.DataAccess;
using LayeredArchitecture.Example.DataAccess.Mappings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LayeredArchitecture.Example
{
    /// <summary>
    /// Configures the Dependency Injection Container.
    /// </summary>
    public static class Bootstrapper
    {
        public static void RegisterServices(HostBuilderContext context, IServiceCollection services)
        {
            // Add Logging Infrastructure:
            services.AddLogging();

            // Entity Type Mappings:
            services.AddSingleton<IEntityTypeMap, AddressTypeMap>();
            services.AddSingleton<IEntityTypeMap, PersonTypeMap>();
            services.AddSingleton<IEntityTypeMap, PersonAddressTypeMap>();

            // Ambient Database:
            ConfigureDbConnectionFactory(services);

            services.AddSingleton<IDbConnectionScopeResolver, DbConnectionScopeProvider>();
            services.AddSingleton<DbConnectionScopeFactory>();
            services.AddSingleton<IUserAccessor, MockUserAccessor>();

            // SQL Client Abstraction:
            services.AddSingleton<SqlClient>();
            services.AddSingleton<SqlQueryContextFactory, SqlServerQueryContextFactory>();

            // Data Access:
            services.AddSingleton<IAddressDao, AddressDao>();
            services.AddSingleton<IPersonDao, PersonDao>();
            services.AddSingleton<IPersonAddressDao, PersonAddressDao>();

            // Services:
            services.AddSingleton<IPersonService, PersonService>();

            // Application:
            services.AddSingleton<Application>();
        }

        private static void ConfigureDbConnectionFactory(IServiceCollection services)
        {
            // A Tenant for our Console Application:
            var tenant = new Tenant
            {
                Id = 1,
                Name = "2DDC0BD3-9C9E-4189-B5B6-B03DF5FA9E87",
                Description = "Console Application"
            };

            // The configuration with the Connection String to a Database:
            var configuration = new TenantConfiguration
            {
                Id = 1,
                TenantId = 1,
                Name = tenant.Name,
                ConnectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=SampleDb;Integrated Security=SSPI;"
            };

            // Always resolves to the App Tenant:
            var resolver = new TenantResolver(tenant);
            var connectionFactory = new SqlServerDbConnectionFactory(resolver, new[] { configuration });

            // Register both:
            services.AddSingleton<ITenantResolver>(resolver);
            services.AddSingleton<IDbConnectionFactory>(connectionFactory);
        }
    }
}
