// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Tenancy;
using LayeredArchitecture.Base.Tenancy.Exceptions;
using LayeredArchitecture.Base.Tenancy.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace LayeredArchitecture.DataAccess.Base
{
    /// <summary>
    /// A default SQL Server-based factory to build <see cref="DbConnection"/> for a Tenant.
    /// </summary>
    public class SqlServerDbConnectionFactory : DbConnectionFactory
    {
        private readonly IDictionary<string, TenantConfiguration> tenants;

        public SqlServerDbConnectionFactory(ITenantResolver tenantResolver, IEnumerable<TenantConfiguration> tenants) 
            : base(tenantResolver)
        {
            this.tenants = tenants.ToDictionary(x => x.Name, x => x);
        }

        public override DbConnection Create(Tenant tenant)
        {
            if(tenant == null)
            {
                throw new ArgumentNullException(nameof(tenant));
            }

            if(!tenants.ContainsKey(tenant.Name))
            {
                throw new MissingTenantConfigurationException($"No TenantConfiguration registered for Tenant '{tenant.Name}'");
            }

            var connectionString = tenants[tenant.Name].ConnectionString;

            if(string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidTenantConfigurationException($"No Connection String registered for Tenant '{tenant.Name}'");
            }

            return new SqlConnection(connectionString);
        }
    }
}
