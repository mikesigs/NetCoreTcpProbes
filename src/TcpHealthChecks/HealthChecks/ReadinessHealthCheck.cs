using Microsoft.Extensions.Hosting;
using TcpHealthChecks.Core;

namespace TcpHealthChecks.HealthChecks
{
    /// <summary>
    /// The default Readiness Health Check.
    /// Is healthy after ApplicationStarted, and unhealthy upon ApplicationStopping or ApplicationStopped.
    /// </summary>
    public class ReadinessHealthCheck : IHealthCheck
    {
        private bool _isReady;

        public ReadinessHealthCheck(IApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(() => _isReady = true);
            lifetime.ApplicationStopping.Register(() => _isReady = false);
            lifetime.ApplicationStopped.Register(() => _isReady = false);
        }

        public HealthCheckKind Kind => HealthCheckKind.Readiness;

        public bool IsHealthy() => _isReady;
    }
}