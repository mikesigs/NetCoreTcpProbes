using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using TcpHealthChecks.Core;
using TcpHealthChecks.Listeners;
using Xunit;

namespace TcpHealthChecks.Tests
{
    public class HealthCheckTcpListener_Should
    {
        private const HealthCheckKind ExpectedKind = HealthCheckKind.Liveness;
        private const HealthCheckKind UnexpectedKind = HealthCheckKind.Readiness;

        private static readonly TcpHealthCheckSettings.HealthCheckSettings Settings =
            new TcpHealthCheckSettings.HealthCheckSettings
            {
                Frequency = TimeSpan.FromSeconds(1),

                // Port 0 causes the TCP listener to use the next available port when the listener starts.
                // The tests must ignore the Range annotation to support test parallelization. 
                Port = 0 
            };

        private readonly Mock<IServiceProvider> _mockServiceProvider;

        public HealthCheckTcpListener_Should()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();

            var mockScope = new Mock<IServiceScope>();
            mockScope
                .Setup(_ => _.ServiceProvider)
                .Returns(_mockServiceProvider.Object);

            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            mockServiceScopeFactory
                .Setup(_ => _.CreateScope())
                .Returns(mockScope.Object);

            Sut = new HealthCheckTcpListener(
                ExpectedKind,
                Settings,
                new Mock<ILogger>().Object,
                mockServiceScopeFactory.Object);
        }

        public HealthCheckTcpListener Sut { get; }

        [Fact]
        public async Task Initially_listen_but_stop_when_status_switches_to_unhealthy()
        {
            // Arrange
            var isHealthy = true;
            // ReSharper disable once AccessToModifiedClosure
            RegisterHealthChecks(new TestHealthCheck(ExpectedKind, () => isHealthy));

            // Act
            await StartListener();

            // Assert
            Sut.IsListening.ShouldBeTrue();
            isHealthy = false;
            await Task.Delay(Settings.Frequency.GetValueOrDefault());
            Sut.IsListening.ShouldBeFalse();
        }

        [Fact]
        public async Task Initially_not_listen_but_start_when_status_switches_to_healthy()
        {
            // Arrange
            var isHealthy = false;
            // ReSharper disable once AccessToModifiedClosure
            RegisterHealthChecks(new TestHealthCheck(ExpectedKind, () => isHealthy));

            // Act
            await StartListener();

            // Assert
            Sut.IsListening.ShouldBeFalse();
            isHealthy = true;
            await Task.Delay(Settings.Frequency.GetValueOrDefault());
            Sut.IsListening.ShouldBeTrue();
        }

        [Fact]
        public async Task Listen_when_all_registered_health_checks_of_expected_kind_return_true()
        {
            // Arrange
            RegisterHealthChecks(new TestHealthCheck(ExpectedKind, () => true));

            // Act
            await StartListener();

            // Assert
            Sut.IsListening.ShouldBeTrue();
        }

        [Fact]
        public async Task Not_listen_when_at_least_one_registered_health_check_of_expected_kind_returns_false()
        {
            // Arrange
            RegisterHealthChecks(
                new TestHealthCheck(ExpectedKind, () => true),
                new TestHealthCheck(ExpectedKind, () => false));

            // Act
            await StartListener();

            // Assert
            Sut.IsListening.ShouldBeFalse();
        }

        [Fact]
        public async Task Not_listen_when_no_registered_health_checks()
        {
            // Arrange
            RegisterHealthChecks();

            // Act
            await StartListener();

            // Assert
            Sut.IsListening.ShouldBeFalse();
        }

        [Fact]
        public async Task Not_listen_when_no_registered_health_checks_of_expected_kind()
        {
            // Arrange
            RegisterHealthChecks(new TestHealthCheck(UnexpectedKind, () => false));

            // Act
            await StartListener();

            // Assert
            Sut.IsListening.ShouldBeFalse();
        }

        private void RegisterHealthChecks(params IHealthCheck[] healthChecks)
        {
            _mockServiceProvider
                .Setup(_ => _.GetService(typeof(IEnumerable<IHealthCheck>)))
                .Returns(healthChecks);
        }

        private async Task StartListener()
        {
            await Sut.StartAsync(default(CancellationToken));
            // Give the listener some time to start
            await Task.Delay(TimeSpan.FromMilliseconds(250));
        }
    }
}