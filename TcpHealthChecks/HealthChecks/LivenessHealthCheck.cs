using TcpHealthChecks.Core;

namespace TcpHealthChecks.HealthChecks
{
    public class LivenessHealthCheck : IHealthCheck
    {
        public HealthCheckKind Kind => HealthCheckKind.Liveness;

        public bool IsHealthy() => true;
    }
}