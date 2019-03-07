namespace TcpHealthChecks
{
    public interface IHealthCheck
    {
        HealthCheckKind Kind { get; }
        bool IsHealthy();
    }
}