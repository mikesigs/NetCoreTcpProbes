using System;
using System.IO;
using System.Threading.Tasks;
using TcpHealthChecks.Core;

namespace CustomHealthChecksExample
{
    public class UnhealthyFileLivenessCheck : IHealthCheck
    {
        static UnhealthyFileLivenessCheck()
        {
            // Creates the 'unhealthy' file 30 seconds after app start
            Task.Delay(TimeSpan.FromSeconds(30))
                .ContinueWith(_ => File.Create("unhealthy"));

        }
        public HealthCheckKind Kind => HealthCheckKind.Liveness;
        public bool IsHealthy() => !File.Exists("unhealthy");
    }
}