using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TcpHealthChecks.Core;

namespace TcpHealthChecks.Listeners
{
    /// <summary>
    /// A background service providing support for TCP based Readiness Health Checks
    /// </summary>
    public class ReadinessTcpListener : HealthCheckTcpListener
    {
        public ReadinessTcpListener(IOptions<TcpHealthCheckSettings> options, ILogger<ReadinessTcpListener> logger, IServiceScopeFactory serviceScopeFactory)
            : base(
                HealthCheckKind.Readiness, 
                options.Value.Readiness,
                logger,
                serviceScopeFactory) {}
    }
}