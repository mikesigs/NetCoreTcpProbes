using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TcpHealthChecks
{
    public class ReadinessHealthCheck : TcpHealthCheck
    {
        public ReadinessHealthCheck(IConfiguration config)
            : base(config.GetValue<int>("HealthChecks:ReadinessPort"), "Readiness") {}
    }
}