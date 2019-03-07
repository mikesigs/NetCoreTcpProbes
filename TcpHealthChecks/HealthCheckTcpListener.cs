using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TcpHealthChecks
{
    public class HealthCheckTcpListener : IHostedService, IDisposable
    {
        private readonly HealthCheckKind _kind;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TcpListener _listener;
        private Timer _timer;

        protected HealthCheckTcpListener(HealthCheckKind kind, int port, IServiceScopeFactory serviceScopeFactory)
        {
            _kind = kind;
            _serviceScopeFactory = serviceScopeFactory;
            _listener = new TcpListener(IPAddress.Any, port);
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

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
                    .Where(_ => _.Kind == _kind);

                if (checks.Any(check => !check.IsHealthy()))
                {
                    Console.WriteLine($"One or more {_kind} checks failed. Stopping TCP Listener.");
                    _listener.Stop();
                }
                else
                {
                    Console.WriteLine($"All {_kind} checks passed. Starting TCP Listener.");
                    _listener.Start();
                }
            }
        }
        
        public void Dispose()
        {
            _listener.Stop();
            _timer.Dispose();
        }
    }
}