// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using LayeredArchitecture.Base.Identity;
using LayeredArchitecture.Base.Serilog;
using LayeredArchitecture.Base.Tenancy;
using LayeredArchitecture.Example.App;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.Threading.Tasks;

namespace LayeredArchitecture.Example
{
    class Program
    {
        private static string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}";

        static async Task Main(string[] args)
        {
            // Now Create the Host:
            using IHost host = CreateHostBuilder(args).Build();

            await host.Services
                .GetRequiredService<Application>()
                .RunAsync();
         
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // Create the Host and Register Dependencies:
            var hostBuilder = Host
                .CreateDefaultBuilder(args)
                .ConfigureServices(RegisterServices)
                .ConfigureLogging(RegisterLogging)
                .UseSerilog((hostingContext, services, loggerConfiguration) =>
                {
                    loggerConfiguration
                                    .MinimumLevel.Verbose()
                                    // Override Minimum Levels:
                                    .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                                    .MinimumLevel.Override("System", LogEventLevel.Verbose)
                                    // Enrich the Logs:
                                    .Enrich.With(
                                        new ProcessIdEnricher(),
                                        new ProcessNameEnricher(),
                                        new ThreadIdEnricher(),
                                        new ThreadCorrelationIdEnricher(),
                                        new MachineNameEnricher(),
                                        new TenantIdEnricher(services.GetRequiredService<ITenantResolver>()),
                                        new TenantNameEnricher(services.GetRequiredService<ITenantResolver>()),
                                        new UserIdEnricher(services.GetRequiredService<IUserAccessor>()))
                                    // Add Log Appenders:
                                    .WriteTo.Console(outputTemplate: outputTemplate)
                                    .WriteTo.File("log.txt",
                                        rollingInterval: RollingInterval.Day,
                                        rollOnFileSizeLimit: true,
                                        outputTemplate: outputTemplate);
                });

            return hostBuilder;
        }

        private static void RegisterLogging(HostBuilderContext context, ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(); 
        }

        public static void RegisterServices(HostBuilderContext context, IServiceCollection services)
        {
            Bootstrapper.RegisterServices(context, services);
        }
    }
}
