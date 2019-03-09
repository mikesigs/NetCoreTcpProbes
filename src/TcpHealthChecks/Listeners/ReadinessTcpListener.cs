using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TcpHealthChecks.Core;

namespace TcpHealthChecks.Listeners
{
    public class ReadinessTcpListener : HealthCheckTcpListener
    {
        public ReadinessTcpListener(IConfiguration config, ILogger<ReadinessTcpListener> logger, IServiceScopeFactory serviceScopeFactory)
            : base(
                HealthCheckKind.Readiness, 
                config.GetValue<int?>("TcpHealthChecks:Readiness:Port") ?? 13001,
                config.GetValue<int?>("TcpHealthChecks:Readiness:Frequency") ?? 5, 
                logger,
                serviceScopeFactory) {}
    }
}