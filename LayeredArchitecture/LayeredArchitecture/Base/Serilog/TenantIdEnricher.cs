// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Tenancy;
using Serilog.Core;
using Serilog.Events;
using System;

namespace LayeredArchitecture.Base.Serilog
{
    /// <summary>
    /// Adds the Tenant ID to a Log entry.
    /// </summary>
    public class TenantIdEnricher : ILogEventEnricher
    {
        public const string TenantIdPropertyName = "TenantId";

        private readonly ITenantResolver tenantResolver;

        public TenantIdEnricher(ITenantResolver tenantResolver)
        {
            this.tenantResolver = tenantResolver;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var logEventProperty = propertyFactory.CreateProperty(TenantIdPropertyName, tenantResolver.Tenant?.Id);

            logEvent.AddPropertyIfAbsent(logEventProperty);
        }
    }

    /// <summary>
    /// Adds the Tenant Name to a Log entry.
    /// </summary>
    public class TenantNameEnricher : ILogEventEnricher
    {
        public const string TenantNamePropertyName = "TenantName";

        private readonly ITenantResolver tenantResolver;

        public TenantNameEnricher(ITenantResolver tenantResolver)
        {
            this.tenantResolver = tenantResolver;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var logEventProperty = propertyFactory.CreateProperty(TenantNamePropertyName, tenantResolver.Tenant?.Name);

            logEvent.AddPropertyIfAbsent(logEventProperty);
        }
    }
}