using Howabout.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Howabout.Controllers
{
	public class SystemController : Controller
	{
		[HttpGet("system/metrics")]
		public async Task<SystemInfo> MetricsAsync()
		{
			return await SystemMetricsRepository.GetMetricsAsync();
		}
	}
}
