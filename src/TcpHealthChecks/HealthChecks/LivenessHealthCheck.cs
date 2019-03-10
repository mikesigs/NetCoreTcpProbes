using TcpHealthChecks.Core;

namespace TcpHealthChecks.HealthChecks
{
    /// <summary>
    /// The default Liveness Health Check
    /// IsHealthy is always true. Basically, if the app is properly configured and running, it's healthy. 
    /// </summary>
    public class LivenessHealthCheck : IHealthCheck
    {
        public HealthCheckKind Kind => HealthCheckKind.Liveness;

        public bool IsHealthy() => true;
    }
}