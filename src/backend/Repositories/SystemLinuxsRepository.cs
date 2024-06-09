
using DocumentFormat.OpenXml.Spreadsheet;
using Howabout.Interfaces;
using Howabout.Models;
using System.Globalization;
using System.IO;
using System.Management;
using System.Runtime.Versioning;

namespace Howabout.Repositories
{
    [SupportedOSPlatform("Linux")]
	public class SystemLinuxRepository : ISystemRepository
	{
		private readonly IShellCommand _shell;
		private readonly CultureInfo _culture = CultureInfo.InvariantCulture; // new CultureInfo("en-US");

		public SystemLinuxRepository(IShellCommand shell)
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
			var result = await _shell.Execute("top", "-bn1");
			var lines = result.Split("\n", StringSplitOptions.RemoveEmptyEntries);
			var cpuLine = lines[2].Split(",", StringSplitOptions.RemoveEmptyEntries);
			var cpuUsage = cpuLine[0]
				.Split(":", StringSplitOptions.RemoveEmptyEntries)[1] // "0.4 us"
				.Replace("us", string.Empty).Trim();                  // "0.4"
			if (float.TryParse(cpuUsage, _culture, out var cpuUsageFloat))      // 0.4
			{
				return (int)(cpuUsageFloat * 100);                    // 40
			}
			return 0;
		}
	
		private async Task<int> GetMemoryAsync()
		{
			var result = await _shell.Execute("free", "-m");
			var lines = result.Split("\n", StringSplitOptions.RemoveEmptyEntries);
			var memoryLine = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
			var total = int.Parse(memoryLine[1]);
			var used = int.Parse(memoryLine[2]);
			var memoryUsage = (used * 100) / total;
			return memoryUsage;
		}
	}
}

/*
us: user cpu time(or) % CPU time spent in user space
sy: system cpu time (or) % CPU time spent in kernel space
ni: user nice cpu time (or) % CPU time spent on low priority processes
id: idle cpu time (or) % CPU time spent idle
wa: io wait cpu time (or) % CPU time spent in wait (on disk)
hi: hardware irq(or) % CPU time spent servicing/handling hardware interrupts
si: software irq(or) % CPU time spent servicing/handling software interrupts
st: steal time - - % CPU time in involuntary wait by virtual cpu while hypervisor is servicing another processor (or) % CPU time stolen from a virtual machine
*/