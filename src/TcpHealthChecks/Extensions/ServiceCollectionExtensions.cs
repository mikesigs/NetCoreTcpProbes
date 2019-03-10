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
            AddLivenessTcpHealthCheck(services);
            AddReadinessTcpHealthCheck(services);
        }

            services.AddSingleton<IHealthCheck, LivenessHealthCheck>();
            services.AddSingleton<IHealthCheck, ReadinessHealthCheck>();
            services.AddHostedService<LivenessTcpListener>();
            services.AddHostedService<ReadinessTcpListener>();
        }
    }
}