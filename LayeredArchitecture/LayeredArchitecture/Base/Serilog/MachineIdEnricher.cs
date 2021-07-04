// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Serilog.Core;
using Serilog.Events;
using System;

namespace LayeredArchitecture.Base.Serilog
{
    /// <summary>
    /// Adds the <see cref="Environment.MachineName"/> to a Log Entry.
    /// </summary>
    public class MachineNameEnricher : ILogEventEnricher
    {
        private LogEventProperty cachedLogEvent;

        public const string MachineNameEnricherPropertyName = "MachineName";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            cachedLogEvent = cachedLogEvent ?? propertyFactory.CreateProperty(MachineNameEnricherPropertyName, Environment.MachineName);

            logEvent.AddPropertyIfAbsent(cachedLogEvent);
        }
    }
}