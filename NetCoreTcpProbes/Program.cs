using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TcpHealthChecks;

namespace NetCoreTcpProbes
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Task.Run(async () =>
            {
                await Task.Delay(4000);
                Console.WriteLine("Setting IsReady to true.");
                IsReady = true;
            });

            Task.Run(async () =>
            {
                await Task.Delay(20000);
                Console.WriteLine("Setting IsLive to false.");
                IsLive = false;
            });

            var builder = new HostBuilder()
                .ConfigureAppConfiguration((context, config) => { config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.AddScoped<IHealthCheck, SelfHealthCheck>();
                    services.AddScoped<IHealthCheck, ReadinessHealthCheck>();
                    services.AddHostedService<LivenessTcpListener>();
                    services.AddHostedService<ReadinessTcpListener>();
                });

            await builder.RunConsoleAsync();
        }
        public static bool IsReady { get; set; }
        public static bool IsLive { get; set; } = true;
    }

    internal class SelfHealthCheck : IHealthCheck
    {
        public HealthCheckKind Kind => HealthCheckKind.Liveness;

        public bool IsHealthy() => Program.IsLive;
    }

    internal class ReadinessHealthCheck : IHealthCheck
    {
        public HealthCheckKind Kind => HealthCheckKind.Readiness;

        public bool IsHealthy() => Program.IsReady;
    }
}