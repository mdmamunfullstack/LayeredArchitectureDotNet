// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Tenancy.Model;

namespace LayeredArchitecture.Base.Tenancy
{
    /// <summary>
    /// An Interface to get the current Tenant.
    /// </summary>
    public interface ITenantResolver
    {
        Tenant Tenant { get; }
    }
}
