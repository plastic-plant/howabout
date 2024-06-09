using Howabout.Models;
using Howabout.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Howabout.Hubs
{
	public class SystemInfoHub: Hub<ISystemInfoHub>
	{
		private readonly ISystemRepository _systemRepository;

		public SystemInfoHub(ISystemRepository systemRepository)
		{
			_systemRepository = systemRepository ?? throw new ArgumentNullException(nameof(systemRepository));
		}

		public async Task<SystemInfo> GetMetrics()
		{
			return await _systemRepository.GetMetricsAsync();
		}
	}

	public interface ISystemInfoHub
	{
		Task<SystemInfo> GetMetrics();
	}
}
