
using System.Management;
using System.Runtime.Versioning;
using Howabout.Models;

namespace Howabout.Repositories
{
    [SupportedOSPlatform("Windows")]
	public class SystemWindowsRepository : ISystemRepository
	{
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
			var tasker = new TaskCompletionSource<int>();
			var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name = '_Total'");

			ManagementObjectCollection? results = null;
			try
			{
				results = await Task.Run(() => searcher.Get());
				foreach (ManagementObject processor in results)
				{
					var idlePercentage = Convert.ToInt32(processor.Properties["PercentIdleTime"].Value);
					tasker.TrySetResult(100 - idlePercentage);
					break;
				}
			}
			catch (Exception ex)
			{
				tasker.TrySetException(ex);
			}
			finally
			{
				results?.Dispose();
				searcher?.Dispose();
			}

			return await tasker.Task;
		}
	
		private async Task<int> GetMemoryAsync()
		{
			var tasker = new TaskCompletionSource<int>();
			var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");

			ManagementObjectCollection? results = null;
			try
			{
				results = await Task.Run(() => searcher.Get());
				foreach (ManagementObject result in results)
				{
					var freePhysicalMemory = Convert.ToDouble(result["FreePhysicalMemory"]);
					var totalVisibleMemorySize = Convert.ToDouble(result["TotalVisibleMemorySize"]);
					var usagePercentage = Convert.ToInt32(Math.Round((freePhysicalMemory / totalVisibleMemorySize) * 100, 0));
					tasker.TrySetResult(usagePercentage);
					break;
				}
			}
			catch (Exception ex)
			{
				tasker.TrySetException(ex);
			}
			finally
			{
				results?.Dispose();
				searcher?.Dispose();
			}

			return await tasker.Task;
		}
	}
}
