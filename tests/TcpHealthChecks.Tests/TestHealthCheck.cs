using System;
using TcpHealthChecks.Core;

namespace TcpHealthChecks.Tests
{
    internal class TestHealthCheck : IHealthCheck
    {
        private readonly Func<bool> _isHealthy;

        public TestHealthCheck(HealthCheckKind kind, Func<bool> isHealthy)
        {
            Kind = kind;
            _isHealthy = isHealthy;
        }

        public HealthCheckKind Kind { get; }

        public bool IsHealthy() => _isHealthy();
    }
}