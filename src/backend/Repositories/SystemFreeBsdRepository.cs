
using Howabout.Interfaces;
using Howabout.Models;
using System.Globalization;
using System.Runtime.Versioning;

namespace Howabout.Repositories
{
    [SupportedOSPlatform("FreeBSD")]
	public class SystemFreeBsdRepository : ISystemRepository
	{
		private readonly IShellCommand _shell;
		private readonly CultureInfo _culture = new CultureInfo("en-US");

		public SystemFreeBsdRepository(IShellCommand shell)
        {
            _shell = shell ?? throw new ArgumentNullException(nameof(shell));
        }

        public async Task<SystemInfo> GetMetricsAsync()
		{
			return new SystemInfo()
			{
				CpuUsagePercentage = await GetCpuAsync(),
				MemoryUsagePercentage = await GetMemoryAsync()
			};
		}
		
		private async Task<int> GetCpuAsync()
		{
			var result = await _shell.Execute("sysctl", "-n dev.cpu.0.cx_usage");
			var cpuIdle = result.Split(" ", StringSplitOptions.RemoveEmptyEntries)[2].Trim('%');
			if (double.TryParse(cpuIdle, _culture, out var cpuIdleDouble)) // "59,99"
			{
				var cpuUsage = 100 - cpuIdleDouble;                        // 40,01
				return (int)Math.Round(cpuUsage, 0);                       // 40
			}

			return 0;
		}
	
		private async Task<int> GetMemoryAsync()
		{
			var result = await _shell.Execute("sysctl", "-n vm.stats.vm.v_page_count vm.stats.vm.v_inactive_count");
			var line = result.Split("\n", StringSplitOptions.RemoveEmptyEntries)[0];
			var pagecount = int.Parse(line.Split(" ")[0]);
			var inactivecount = int.Parse(line.Split(" ")[1]);
			var used = pagecount - inactivecount;
			var memoryUsage = (used * 100) / pagecount;
			return memoryUsage;
		}
	}
}
