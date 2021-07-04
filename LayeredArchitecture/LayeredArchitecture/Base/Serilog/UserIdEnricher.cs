// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Exensions;
using LayeredArchitecture.Base.Identity;
using Serilog.Core;
using Serilog.Events;
using System;

namespace LayeredArchitecture.Base.Serilog
{
    /// <summary>
    /// Adds the Thread ID of the <see cref="Environment"/> to the Log entry.
    /// </summary>
    public class UserIdEnricher : ILogEventEnricher
    {
        public const string UserIdPropertyName = "UserId";

        private readonly IUserAccessor userAccessor;


        private string cachedUserId;
        private LogEventProperty cachedLogEvent;

        public UserIdEnricher(IUserAccessor userAccessor)
        {
            this.userAccessor = userAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var currentUserId = userAccessor.User.GetUserId();

            if (cachedLogEvent == null || !string.Equals(cachedUserId, currentUserId, StringComparison.InvariantCulture))
            {
                cachedUserId = currentUserId;
                cachedLogEvent = cachedLogEvent ?? propertyFactory.CreateProperty(UserIdPropertyName, currentUserId);
            }

            var logEventProperty = propertyFactory.CreateProperty(UserIdPropertyName, cachedLogEvent);

            logEvent.AddPropertyIfAbsent(logEventProperty);
        }
    }
}