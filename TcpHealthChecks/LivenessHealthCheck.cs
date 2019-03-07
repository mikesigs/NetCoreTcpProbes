using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TcpHealthChecks
{
    public class LivenessHealthCheck : TcpHealthCheck
    {
        public LivenessHealthCheck(IConfiguration config)
            : base(config.GetValue<int>("HealthChecks:LivenessPort"), "Liveness") {}
    }
}