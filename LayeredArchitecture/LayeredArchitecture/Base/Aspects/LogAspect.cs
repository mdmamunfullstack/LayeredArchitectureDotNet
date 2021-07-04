// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ArxOne.MrAdvice.Advice;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace LayeredArchitecture.Base.Aspects
{
    /// <summary>
    /// Advises a method for Trace Logging using the MrAdvice Framework. It excludes any Logging for Constructor Invocations. 
    /// 
    /// This enables to get a verbose trace, if you for example need to trace errors in a Remote System.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class LogAspectAttribute : Attribute, IMethodAsyncAdvice
    {
        public async Task Advise(MethodAsyncAdviceContext context)
        {
            if (!context.TargetMethod.IsConstructor)
            {
                if (Log.IsEnabled(LogEventLevel.Verbose))
                {
                    Log.Logger.Verbose("Entering Method {DeclaringType}.{Method} (Arguments {@Arguments})", context.TargetMethod.DeclaringType.FullName, context.TargetMethod.Name, context.Arguments);
                }
            }

            await context.ProceedAsync();

            if (!context.TargetMethod.IsConstructor)
            {
                if (Log.IsEnabled(LogEventLevel.Verbose))
                {
                    Log.Logger.Verbose("Exiting Method {DeclaringType}.{Method}", context.TargetMethod.Name, context.Arguments);
                }
            }
        }
    }
}
