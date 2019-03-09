using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpHealthChecks.Core
{
    public class HealthCheckSettings
    {
        private int LivenessPort { get; set; }
        private int ReadinessPort { get; set; }
    }
}
