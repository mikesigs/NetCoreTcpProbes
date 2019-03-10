using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace TcpHealthChecks.Core
{
    /// <summary>
    /// Settings required by the TCP Health Checks
    /// </summary>
    public class TcpHealthCheckSettings
    {
        /// <summary>
        /// Liveness Health Check Settings
        /// </summary>
        public HealthCheckSettings Liveness { get; set; }
        
        /// <summary>
        /// Readiness Health Check Settings
        /// </summary>
        public HealthCheckSettings Readiness { get; set; }

        public class HealthCheckSettings
        {
            /// <summary>
            /// The port to use for this Health Check.
            /// <value>Must be greater than 0 and less than or equal to IPEndPoint.MaxPort</value>
            /// </summary>
            [Range(IPEndPoint.MinPort + 1, IPEndPoint.MaxPort)]
            public int Port { get; set; }

            public TimeSpan? Frequency { get; set; }
        }
    }
}