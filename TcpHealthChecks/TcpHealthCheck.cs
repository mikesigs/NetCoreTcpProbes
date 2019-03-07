using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace TcpHealthChecks
{
    public abstract class TcpHealthCheck : BackgroundService
    {
        private readonly string _type;
        private readonly TcpListener _listener;

        protected TcpHealthCheck(int port, string type)
        {
            _type = type;
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _listener.Start();
            cancellationToken.Register(() => Console.WriteLine($"{nameof(TcpHealthCheck)}.StartAsync for type: {_type} canceled."));
            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => Console.WriteLine($"{nameof(TcpHealthCheck)}.StopAsync for type: {_type} canceled."));
            await base.StopAsync(cancellationToken);
            _listener.Stop();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bytes = new byte[256];

            while (!stoppingToken.IsCancellationRequested)
            {
                // Perform a blocking call to accept requests.
                var client = await _listener.AcceptTcpClientAsync();
                Console.WriteLine($"{DateTime.Now.ToUniversalTime()} {_type} check received.");
                client.Close();
            }
        }
    }
}