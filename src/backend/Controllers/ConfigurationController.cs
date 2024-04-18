using Howabout.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Howabout.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ConfigurationController : Controller
	{
		private readonly ILogger<ConfigurationController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IHostApplicationLifetime _lifetime;
		private readonly IKernelMemoryService _kernelMemoryService;

		public ConfigurationController(ILogger<ConfigurationController> logger, IConfiguration configuration, IHostApplicationLifetime lifeTime, IKernelMemoryService kernelMemoryService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_lifetime = lifeTime ?? throw new ArgumentNullException(nameof(lifeTime));
			_kernelMemoryService = kernelMemoryService ?? throw new ArgumentNullException(nameof(kernelMemoryService));
		}

		[HttpGet("reload")]
		public bool Reload()
		{
			_kernelMemoryService.Configure();
			return true;
		}

		[HttpGet("stop")]
		public bool Stop()
		{
			_lifetime.StopApplication();
			return true;
		}

		[HttpGet("bebop")]
		public async Task<string> QuickProofOfLifeAsync()
		{
			var memory = _kernelMemoryService.Get();
			await memory.ImportTextAsync("Bebop or bop is a style of jazz developed in the early to mid-1940s in the United States. The style features compositions characterized by a fast tempo (usually exceeding 200 bpm), complex chord progressions with rapid chord changes and numerous changes of key, instrumental virtuosity, and improvisation based on a combination of harmonic structure, the use of scales and occasional references to the melody.\r\n\r\nBebop developed as the younger generation of jazz musicians expanded the creative possibilities of jazz beyond the popular, dance-oriented swing music-style with a new \"musician's music\" that was not as danceable and demanded close listening.[1] As bebop was not intended for dancing, it enabled the musicians to play at faster tempos. Bebop musicians explored advanced harmonies, complex syncopation, altered chords, extended chords, chord substitutions, asymmetrical phrasing, and intricate melodies. Bebop groups used rhythm sections in a way that expanded their role. Whereas the key ensemble of the swing music era was the big band of up to fourteen pieces playing in an ensemble-based style, the classic bebop group was a small combo that consisted of saxophone (alto or tenor), trumpet, piano, guitar, double bass, and drums playing music in which the ensemble played a supportive role for soloists. Rather than play heavily arranged music, bebop musicians typically played the melody of a composition (called the \"head\") with the accompaniment of the rhythm section, followed by a section in which each of the performers improvised a solo, then returned to the melody at the end of the composition. Some of the most influential bebop artists, who were typically composer-performers, are alto sax player Charlie Parker; tenor sax players Dexter Gordon, Sonny Rollins, and James Moody; clarinet player Buddy DeFranco; trumpeters Fats Navarro, Clifford Brown, Miles Davis, and Dizzy Gillespie; pianists Bud Powell, Barry Harris and Thelonious Monk; electric guitarist Charlie Christian; and drummers Kenny Clarke, Max Roach, and Art Blakey.");
			var answer = await memory.AskAsync("How fast do we bop?"); // 200 bpm or higher.
			return answer.Result;
		}
	}
}
