using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TcpHealthChecks;

namespace NetCoreTcpProbes
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.AddHostedService<LivenessHealthCheck>();
                    services.AddHostedService<ReadinessHealthCheck>();
                });

            await builder.RunConsoleAsync();
        }
    }
}
