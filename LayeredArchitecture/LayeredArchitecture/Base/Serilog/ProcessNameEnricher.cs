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
    /// Adds a the current Name of the <see cref="System.Diagnostics.Process"/> to the Log entry.
    /// </summary>
    public class ProcessNameEnricher : ILogEventEnricher
    {
        LogEventProperty cachedLogEvent;

        public const string ProcessNamePropertyName = "ProcessName";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var currentProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

            cachedLogEvent = cachedLogEvent ?? propertyFactory.CreateProperty(ProcessNamePropertyName, currentProcessName);

            logEvent.AddPropertyIfAbsent(cachedLogEvent);
        }
    }
}
