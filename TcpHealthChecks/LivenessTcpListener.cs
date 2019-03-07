using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TcpHealthChecks
{
    public class LivenessTcpListener : HealthCheckTcpListener
    {
        public LivenessTcpListener(IConfiguration config, IServiceScopeFactory serviceScopeFactory)
            : base(HealthCheckKind.Liveness, config.GetValue<int>("HealthChecks:LivenessPort"), serviceScopeFactory) {}
    }
}