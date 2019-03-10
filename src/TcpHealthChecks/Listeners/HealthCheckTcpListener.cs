using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TcpHealthChecks.Core;

namespace TcpHealthChecks.Listeners
{
    // TODO: mikes (2019-03-09) - Instead of checking on a timer, maybe do it only when the client attaches.
    // The timeoutSeconds property of k8s probes might help make this work. We need to kill the port before
    // the timeout is exceeded. So checks have to be doable within that timeout period. 
    // That is of course if that is even what the timeoutSeconds property means!
    public class HealthCheckTcpListener : IHostedService, IDisposable
    {
        private readonly TimeSpan _frequency;
        private readonly HealthCheckKind _kind;
        private readonly ILogger _logger;
        private readonly int _port;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;
        private readonly TcpListener _listener;
        protected static readonly TimeSpan DefaultFrequency = TimeSpan.FromSeconds(5);

        public HealthCheckTcpListener(HealthCheckKind kind, TcpHealthCheckSettings.HealthCheckSettings settings, ILogger logger, IServiceScopeFactory serviceScopeFactory)
        {
            _kind = kind;
            _port = settings.Port;
            _listener = new TcpListener(IPAddress.Any, _port);
            _frequency = settings.Frequency ?? DefaultFrequency;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public bool IsListening => _listener.Server.IsBound;

        public void Dispose()
        {
            _listener.Stop();
            _timer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, _frequency);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _listener.Stop();

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var checks = scope.ServiceProvider
                    .GetServices<IHealthCheck>()
                    .Where(_ => _.Kind == _kind)
                    .ToList();

                if (!checks.Any())
                {
                    _logger.LogWarning("No IHealthCheck types with Kind={Kind} are registered with the ServiceProvider. TCP Listener on port {Port} will be permanently stopped.", _kind, _port);
                }
                else if (checks.Any(check => !check.IsHealthy()))
                {
                    _logger.LogDebug("One or more {Kind} checks failed. Ensuring TCP Listener on port {Port} is stopped.", _kind, _port);
                    if (IsListening)
                    {
                        _listener.Stop();
                    }
                }
                else
                {
                    _logger.LogDebug("All {Kind} checks passed. Ensuring TCP Listener on port {Port} is started.", _kind, _port);
                    if (!IsListening)
                    {
                        _listener.Start();
                    }
                }
            }
        }
    }
}