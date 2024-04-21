using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Howabout.Repositories
{
	public static class SystemMetricsRepository
	{
		public static async Task<SystemInfo> GetMetricsAsync()
		{
			var metrics = new SystemInfo();

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				throw new NotImplementedException("Linux not implemented yet");
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				throw new NotImplementedException("macOS not implemented yet");
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				metrics.CpuUsagePercentage = await GetCpuUsagePercentageWindowsAsync();
				metrics.MemoryUsagePercentage = await GetMemoryUsagePercentageWindowsAsync();
			}

			return metrics;
		}

		[SupportedOSPlatform("WINDOWS")]
		public static async Task<int> GetCpuUsagePercentageWindowsAsync()
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

		[SupportedOSPlatform("WINDOWS")]
		public static async Task<int> GetMemoryUsagePercentageWindowsAsync()
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


public class SystemInfo
{
    public int CpuUsagePercentage { get; set; }
	public int MemoryUsagePercentage { get; set; }
}
