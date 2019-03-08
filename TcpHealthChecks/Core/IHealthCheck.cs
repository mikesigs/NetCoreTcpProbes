namespace TcpHealthChecks.Core
{
    public interface IHealthCheck
    {
        HealthCheckKind Kind { get; }
        bool IsHealthy();
    }
}