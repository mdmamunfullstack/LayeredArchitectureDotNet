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
    /// Adds the Thread ID of the <see cref="Environment"/> to the Log entry.
    /// </summary>
    public class ThreadIdEnricher : ILogEventEnricher
    {
        public const string ThreadIdPropertyName = "ThreadId";

        private int cachedThreadId;
        private LogEventProperty cachedLogEvent;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var currentThreadId = Environment.CurrentManagedThreadId;
            
            if (cachedLogEvent == null || cachedThreadId != currentThreadId)
            {
                cachedThreadId = currentThreadId;
                cachedLogEvent = cachedLogEvent ?? propertyFactory.CreateProperty(ThreadIdPropertyName, currentThreadId);
            }

            logEvent.AddPropertyIfAbsent(cachedLogEvent);
        }
    }
}