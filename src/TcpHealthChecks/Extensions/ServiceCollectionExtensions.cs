using System.Net;
using Microsoft.Extensions.DependencyInjection;
using TcpHealthChecks.Core;
using TcpHealthChecks.HealthChecks;
using TcpHealthChecks.Listeners;

namespace TcpHealthChecks.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTcpHealthChecks(this IServiceCollection services)
        {
            // TODO: mikes (2019-03-10) - Should we validate this here? Should this package take a dependency on Options at all???
            services
                .AddOptions<HealthCheckSettings>()
                .Validate(settings =>
                    IPEndPoint.MinPort > settings.LivenessPort && settings.LivenessPort < IPEndPoint.MaxPort)
                .Validate(settings =>
                    IPEndPoint.MinPort > settings.ReadinessPort && settings.ReadinessPort < IPEndPoint.MaxPort, $"{nameof(HealthCheckSettings.ReadinessPort)} is not a valid port number.");

            services.AddSingleton<IHealthCheck, LivenessHealthCheck>();
            services.AddSingleton<IHealthCheck, ReadinessHealthCheck>();
            services.AddHostedService<LivenessTcpListener>();
            services.AddHostedService<ReadinessTcpListener>();
        }
    }
}