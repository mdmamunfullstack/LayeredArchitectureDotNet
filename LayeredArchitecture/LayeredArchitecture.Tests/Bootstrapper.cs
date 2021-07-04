using LayeredArchitecture.Base.Tenancy.Model;
using LayeredArchitecture.DataAccess.Base;
using LayeredArchitecture.DataAccess.Mapping;
using LayeredArchitecture.DataAccess.Sql;
using LayeredArchitecture.Tests.Base;
using LayeredArchitecture.Tests.Business;
using LayeredArchitecture.Tests.DataAccess;
using LayeredArchitecture.Tests.DataAccess.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace LayeredArchitecture.Tests
{
    public static class Bootstrapper
    {
        public static void RegisterDependencies(ServiceCollection services)
        {

            // Entity Type Mappings:
            services.AddSingleton<IEntityTypeMap, PersonTypeMap>();

            // Ambient Database:
            services.AddSingleton<IDbConnectionFactory>(GetTenantDbConnectionFactory());
            services.AddSingleton<IDbConnectionScopeResolver, DbConnectionScopeProvider>();
            services.AddSingleton<DbConnectionScopeFactory>();

            // SQL Client Abstraction:
            services.AddSingleton<SqlClient>();
            services.AddSingleton<SqlQueryContextFactory, SqlServerQueryContextFactory>();

            // Data Access:
            services.AddSingleton<IPersonDao, PersonDao>();

            // Business Services:
            services.AddSingleton<IPersonService, PersonService>();
        }

        private static IDbConnectionFactory GetTenantDbConnectionFactory()
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

            return new SqlServerDbConnectionFactory(resolver, new[] { configuration });
        }
    }
}
