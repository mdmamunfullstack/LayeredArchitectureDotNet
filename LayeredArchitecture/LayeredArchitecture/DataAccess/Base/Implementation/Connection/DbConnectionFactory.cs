
using LayeredArchitecture.Base.Tenancy;
using LayeredArchitecture.Base.Tenancy.Exceptions;
using LayeredArchitecture.Base.Tenancy.Model;
using System.Data.Common;

namespace LayeredArchitecture.DataAccess.Base
{
    /// <summary>
    /// Creates a new <see cref="DbConnection"/ >based on a <see cref="Tenant"/> resolved from a 
    /// <see cref="ITenantResolver"/>, which could resolve the <see cref="Tenant"/> from a HTTP 
    /// Request or set it static somewhere.
    /// </summary>
    public abstract class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly ITenantResolver tenantResolver;

        public DbConnectionFactory(ITenantResolver tenantResolver)
        {
            this.tenantResolver = tenantResolver;
        }

        public DbConnection Create()
        {
            var tenant = tenantResolver.Tenant;

            if(tenant == null)
            {
                throw new MissingTenantException();
            }

            return Create(tenant);
        }

        public abstract DbConnection Create(Tenant tenant);
    }
}
