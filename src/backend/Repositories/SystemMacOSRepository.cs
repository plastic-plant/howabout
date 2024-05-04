
using Howabout.Interfaces;
using Howabout.Models;
using System.Management;
using System.Runtime.Versioning;

namespace Howabout.Repositories
{
    [SupportedOSPlatform("macOS")]
	public class SystemMacOSRepository : ISystemRepository
	{
		private readonly IShellCommand _shell;

		public SystemMacOSRepository(IShellCommand shell)
		{
			_shell = shell ?? throw new ArgumentNullException(nameof(shell));
		}

		public async Task<SystemInfo> GetMetricsAsync()
		{
			var (cpu, memory) = await GetCpuAndMemoryAsync();
			return new SystemInfo()
			{	
				CpuUsagePercentage = cpu,
				MemoryUsagePercentage = memory
			};
		}
		
		private async Task<(int, int)> GetCpuAndMemoryAsync()
		{
			var result = await _shell.Execute("top", "-l 1 -n 0");

			// CPU usage: 38% user, 12% sys, 60% idle
			var lines = result.Split("\n", StringSplitOptions.RemoveEmptyEntries);
			var cpuLine = lines[3].Split(",", StringSplitOptions.RemoveEmptyEntries);
			var cpuIdle = cpuLine[2].Split("%", StringSplitOptions.RemoveEmptyEntries)[0];
			var cpuUsagePercentage = 100 - int.Parse(cpuIdle);

			// PhysMem: 9G used (2304M wired), 6G unused.
			var memoryLine = lines[6].Split(" ", StringSplitOptions.RemoveEmptyEntries);
			var memoryUsed = double.Parse(memoryLine[1].Split(new char[] { 'M', 'G' }, StringSplitOptions.RemoveEmptyEntries)[0]);
			var memoryUnused = double.Parse(memoryLine[5].Split(new char[] { 'M', 'G' }, StringSplitOptions.RemoveEmptyEntries)[0]);
			var totalMemory = memoryUsed + memoryUnused;
			var memoryUsagePercentage = (int)Math.Round((memoryUsed / totalMemory) * 100, 0);
			return (cpuUsagePercentage, memoryUsagePercentage);
		}
		}
}
