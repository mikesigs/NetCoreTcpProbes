using System;
using System.IO;
using System.Threading.Tasks;
using TcpHealthChecks.Core;

namespace CustomHealthChecksExample
{
    public class ReadyFileReadinessCheck : IHealthCheck
    {
        static ReadyFileReadinessCheck()
        {
            // Creates the 'ready' file 10 seconds after app start
            Task.Delay(TimeSpan.FromSeconds(10))
                .ContinueWith(_ => File.Create("ready"));
        }

        public HealthCheckKind Kind => HealthCheckKind.Readiness;
        public bool IsHealthy() => File.Exists("ready");
    }
}