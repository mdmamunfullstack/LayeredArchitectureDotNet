// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace LayeredArchitecture.Base.Tenancy.Model
{
    /// <summary>
    /// Settings for the <see cref="Tenant"/>, focus on Database here.
    /// </summary>
    public class TenantConfiguration
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique Name of the Tenant.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Connection String for Tenants database.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Reference to the Tenant.
        /// </summary>
        public int TenantId { get; set; }
    }
}
