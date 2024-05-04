using Howabout.Interfaces;
using Howabout.Repositories;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Howabout.Tests.Repositories
{
	public class SystemFreeBsdRepositoryTests
	{
		[Fact]
		[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Testing compatibility without executing shell")]
		public async Task GetMetricsAsync_Returns()
		{
			// Arrange
			var shell = new Mock<IShellCommand>();
			shell.Setup(mock => mock.Execute("sysctl", "-n dev.cpu.0.cx_usage")).ReturnsAsync(ExampleCpuTop);
			shell.Setup(mock => mock.Execute("sysctl", "-n vm.stats.vm.v_page_count vm.stats.vm.v_inactive_count")).ReturnsAsync(ExampleMemoryFree);
			var sut = new SystemFreeBsdRepository(shell.Object);

			// Act
			var actual = await sut.GetMetricsAsync();

			// Assert
			Assert.NotNull(actual);
			Assert.Equal(40, actual.CpuUsagePercentage);
			Assert.Equal(3, actual.MemoryUsagePercentage);
		}

		private const string ExampleCpuTop = @"0.00% 0.00% 59.99%";

		private const string ExampleMemoryFree = @"10000 9700";
	}
}
