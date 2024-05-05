using Howabout.Interfaces;
using Howabout.Models;
using Howabout.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Howabout.Controllers
{
    public class SystemController : Controller
	{
		private readonly ILogger<SystemController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IHostApplicationLifetime _lifetime;
		private readonly IKernelMemoryService _kernelMemoryService;
		private readonly ISystemRepository _systemRepository;

        public SystemController(ILogger<SystemController> logger, IConfiguration configuration, IHostApplicationLifetime lifeTime, IKernelMemoryService kernelMemoryService, ISystemRepository systemRepository)
        {
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_lifetime = lifeTime ?? throw new ArgumentNullException(nameof(lifeTime));
			_kernelMemoryService = kernelMemoryService ?? throw new ArgumentNullException(nameof(kernelMemoryService));
			_systemRepository = systemRepository ?? throw new ArgumentNullException(nameof(systemRepository));
        }

        [HttpGet("api/metrics")]
		public async Task<SystemInfo> MetricsAsync()
		{
			return await _systemRepository.GetMetricsAsync();
		}

		[HttpGet("api/reload")]
		public bool Reload()
		{
			_logger.LogInformation("Reload requested.");
			_kernelMemoryService.Configure();
			return true;
		}

		[HttpGet("api/stop")]
		public bool Stop()
		{
			_logger.LogInformation("Stop requested.");
			_lifetime.StopApplication();
			return true;
		}

	}
}
