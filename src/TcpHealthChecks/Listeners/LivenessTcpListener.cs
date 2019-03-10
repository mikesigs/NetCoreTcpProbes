using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TcpHealthChecks.Core;

namespace TcpHealthChecks.Listeners
{
    public class LivenessTcpListener : HealthCheckTcpListener
    {
        public LivenessTcpListener(IOptions<TcpHealthCheckSettings> options, ILogger<LivenessTcpListener> logger, IServiceScopeFactory serviceScopeFactory)
            : base(
                HealthCheckKind.Liveness,
                options.Value.Liveness,
                logger,
                serviceScopeFactory) {}
    }
}