// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;

namespace LayeredArchitecture.Base.Serilog
{
    /// <summary>
    /// Adds a the current Id of the <see cref="System.Diagnostics.Process"/> to the Log entry.
    /// </summary>
    public class ProcessIdEnricher : ILogEventEnricher
    {
        LogEventProperty cachedLogEvent;

        public const string ProcessIdPropertyName = "ProcessId";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var currentProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;

            cachedLogEvent = cachedLogEvent ?? propertyFactory.CreateProperty(ProcessIdPropertyName, currentProcessId);

            logEvent.AddPropertyIfAbsent(cachedLogEvent);
        }
    }
}
