using Howabout.Models;
using Howabout.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Howabout.Controllers
{
    public class SystemController : Controller
	{
		private readonly ISystemRepository _system;

        public SystemController(ISystemRepository system)
        {
			_system = system ?? throw new ArgumentNullException(nameof(system));
        }

        [HttpGet("system/metrics")]
		public async Task<SystemInfo> MetricsAsync()
		{
			return await _system.GetMetricsAsync();
		}
	}
}
