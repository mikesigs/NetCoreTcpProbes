using System.IO;
using TcpHealthChecks.Core;

namespace CustomHealthChecksExample
{
    public class ReadyFileReadinessCheck : IHealthCheck
    {
        public HealthCheckKind Kind => HealthCheckKind.Readiness;
        public bool IsHealthy() => File.Exists("ready");
    }
}