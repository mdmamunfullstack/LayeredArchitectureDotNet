using LayeredArchitecture.Base.Tenancy;
using LayeredArchitecture.Base.Tenancy.Model;

namespace LayeredArchitecture.Tests.Base
{
    /// <summary>
    /// A "useless" Tenant resolver, which always resolves to a single Tenant.
    /// </summary>
    public class TenantResolver : ITenantResolver
    {
        private Tenant tenant;

        public TenantResolver(Tenant tenant)
        {
            this.tenant = tenant;
        }

        public Tenant Tenant => tenant;
    }
}
