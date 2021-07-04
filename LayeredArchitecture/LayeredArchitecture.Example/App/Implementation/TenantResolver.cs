// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Tenancy;
using LayeredArchitecture.Base.Tenancy.Model;

namespace LayeredArchitecture.Example.App.Implementation
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
