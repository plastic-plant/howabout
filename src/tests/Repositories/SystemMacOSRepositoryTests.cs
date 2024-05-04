using Howabout.Interfaces;
using Howabout.Repositories;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Howabout.Tests.Repositories
{
	public class SystemMacOSRepositoryTests
	{
		[Fact]
		[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Testing compatibility without executing shell")]
		public async Task GetMetricsAsync_Returns()
		{
			// Arrange
			var shell = new Mock<IShellCommand>();
			shell.Setup(mock => mock.Execute("top", "-l 1 -n 0")).ReturnsAsync(ExampleTop);
			var sut = new SystemMacOSRepository(shell.Object);

			// Act
			var actual = await sut.GetMetricsAsync();

			// Assert
			Assert.NotNull(actual);
			Assert.Equal(40, actual.CpuUsagePercentage);
			Assert.Equal(3, actual.MemoryUsagePercentage);
		}

		private const string ExampleTop = @"Processes: 326 total, 2 running, 324 sleeping, 1663 threads 
2024/05/04 16:23:52
Load Avg: 2.22, 1.92, 1.75 
CPU usage: 38% user, 12% sys, 60% idle 
SharedLibs: 133M resident, 50M data, 16M linkedit.
MemRegions: 97641 total, 2048M resident, 500M private, 2200M shared.
PhysMem: 3G used (2304M wired), 97G unused.
VM: 2632G vsize, 1128M framework vsize, 25744602(0) swapins, 29261140(0) swapouts.
Networks: packets: 2437588/1866M in, 2830983/1110M out.
Disks: 2888901/84G read, 2013939/99G written.
";
	}
}
