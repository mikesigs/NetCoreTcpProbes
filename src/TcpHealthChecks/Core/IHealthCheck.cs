namespace TcpHealthChecks.Core
{
    /// <summary>
    /// Implement this interface to define custom health checks
    /// </summary>
    public interface IHealthCheck
    {
        /// <summary>
        /// Indicates the type of health check.
        /// <value>Either Readiness or Liveness</value>
        /// </summary>
        HealthCheckKind Kind { get; }

        /// <summary>
        /// Determines if the health check is healthy.
        /// </summary>
        /// <returns>Returns true if the health check is satisfied.</returns>
        bool IsHealthy();
    }
}