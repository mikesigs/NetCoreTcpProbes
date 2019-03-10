using System.Net;
using Microsoft.Extensions.DependencyInjection;
using TcpHealthChecks.Core;
using TcpHealthChecks.HealthChecks;
using TcpHealthChecks.Listeners;

namespace TcpHealthChecks.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Enables both Readiness and Liveness TCP Listeners with default Health Checks
        /// </summary>
        /// <param name="services"></param>
        public static void AddTcpHealthChecks(this IServiceCollection services)
        {
            AddLivenessTcpHealthCheck(services);
            AddReadinessTcpHealthCheck(services);
        }

        /// <summary>
        /// Enables Liveness TCP Listener with default Liveness Health Check
        /// </summary>
        /// <param name="services"></param>
        public static void AddLivenessTcpHealthCheck(IServiceCollection services)
        {
            services.AddSingleton<IHealthCheck, LivenessHealthCheck>();
            services.AddHostedService<LivenessTcpListener>();
        }

        /// <summary>
        /// Enables Readiness TCP Listener with default Readiness Health Check
        /// </summary>
        /// <param name="services"></param>
        public static void AddReadinessTcpHealthCheck(IServiceCollection services)
        {
            services.AddSingleton<IHealthCheck, ReadinessHealthCheck>();
            services.AddHostedService<ReadinessTcpListener>();
        }
    }
}