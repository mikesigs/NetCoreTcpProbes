using System.IO;
using TcpHealthChecks.Core;

namespace Example
{
    public class UnhealthyFileLivenessCheck : IHealthCheck
    {
        public HealthCheckKind Kind => HealthCheckKind.Liveness;
        public bool IsHealthy() => !File.Exists("unhealthy");
    }
}