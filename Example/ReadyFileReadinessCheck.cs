using System.IO;
using TcpHealthChecks.Core;

namespace Example
{
    public class ReadyFileReadinessCheck : IHealthCheck
    {
        public HealthCheckKind Kind => HealthCheckKind.Readiness;
        public bool IsHealthy() => File.Exists("ready");
    }
}