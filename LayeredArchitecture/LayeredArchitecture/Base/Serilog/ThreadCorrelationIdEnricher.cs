// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Threading;

namespace LayeredArchitecture.Base.Serilog
{
    /// <summary>
    /// Adds a random Correlation ID to the Log entries, so we can correlate async flows, where the 
    /// Continuation may be executed on different threads.
    /// </summary>
    public class ThreadCorrelationIdEnricher : ILogEventEnricher
    {
        private static readonly AsyncLocal<Guid> CorrelationIdContext = new AsyncLocal<Guid>();

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (CorrelationIdContext.Value == Guid.Empty)
            {
                CorrelationIdContext.Value = Guid.NewGuid();
            }

            logEvent.AddOrUpdateProperty(new LogEventProperty("ThreadCorrelationId", new ScalarValue(CorrelationIdContext.Value)));
        }
    }
}
