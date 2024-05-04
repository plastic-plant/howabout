using Howabout.Interfaces;
using Howabout.Repositories;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Howabout.Tests.Repositories
{
	public class SystemMetricsLinuxsRepositoryTests
	{
		[Fact]
		[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Testing compatibility without executing shell")]
		public async Task GetMetricsAsync_Returns()
		{
			// Arrange
			var shell = new Mock<IShellCommand>();
			shell.Setup(mock => mock.Execute("top", "-bn1")).ReturnsAsync(ExampleCpuTop);
			shell.Setup(mock => mock.Execute("free", "-m")).ReturnsAsync(ExampleMemoryFree);
			var sut = new SystemLinuxRepository(shell.Object);

			// Act
			var actual = await sut.GetMetricsAsync();

			// Assert
			Assert.NotNull(actual);
			Assert.Equal(40, actual.CpuUsagePercentage);
			Assert.Equal(3, actual.MemoryUsagePercentage);
		}

		private const string ExampleCpuTop = @"top - 13:46:37 up  3:46,  1 user,  load average: 0.07, 0.04, 0.01
Tasks:  33 total,   1 running,  32 sleeping,   0 stopped,   0 zombie
%Cpu(s):  0.4 us,  0.0 sy,  0.0 ni, 99.6 id,  0.0 wa,  0.0 hi,  0.0 si,  0.0 st
MiB Mem :  15348.3 total,  13906.6 free,    564.7 used,    877.0 buff/cache
MiB Swap:   4096.0 total,   4096.0 free,      0.0 used.  14519.6 avail Mem

    PID USER      PR  NI    VIRT    RES    SHR S  %CPU  %MEM     TIME+ COMMAND
    135 root      20   0 2132976  45364  19292 S   6.2   0.3   0:02.90 snapd
      1 root      20   0  166996  12656   8368 S   0.0   0.1   1:22.77 systemd
      2 root      20   0    2460   1344   1224 S   0.0   0.0   0:00.00 init-systemd(Ub
      7 root      20   0    2460      4      0 S   0.0   0.0   0:00.00 init
    826 root      20   0   44176  38544  10268 S   0.0   0.2   0:54.27 python3
  52242 patrick   20   0    7808   3280   2920 R   0.0   0.0   0:00.00 top
";

		/// <summary>
		/// free --version: free from procps-ng 3.3.17
		/// </summary>
		private const string ExampleMemoryFree = @"total        used        free      shared  buff/cache   available
Mem:           15348         571       13899           3         877       14512
Swap:           4096           0        4096
";
	}
}
