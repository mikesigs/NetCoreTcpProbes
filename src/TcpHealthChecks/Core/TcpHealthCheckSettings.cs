using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace TcpHealthChecks.Core
{
    public class TcpHealthCheckSettings
    {
        public HealthCheckSettings Liveness { get; set; }
        public HealthCheckSettings Readiness { get; set; }

        public class HealthCheckSettings
        {
            [Range(IPEndPoint.MinPort + 1, IPEndPoint.MaxPort)]
            public int Port { get; set; }

            public TimeSpan? Frequency { get; set; }
        }
    }
}