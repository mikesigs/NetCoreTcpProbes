using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TcpHealthChecks
{
    public class ReadinessTcpListener : HealthCheckTcpListener
    {
        public ReadinessTcpListener(IConfiguration config, IServiceScopeFactory serviceScopeFactory)
            : base(HealthCheckKind.Readiness, config.GetValue<int>("HealthChecks:ReadinessPort"), serviceScopeFactory) {}
    }
}