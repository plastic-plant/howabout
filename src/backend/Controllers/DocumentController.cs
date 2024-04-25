using Howabout.Configuration;
using Howabout.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Howabout.Controllers
{
	public class DocumentController : Controller
	{
		private readonly ILogger<DocumentController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IKernelMemoryService _kernelMemoryService;

		public DocumentController(ILogger<DocumentController> logger, IConfiguration configuration, IHostApplicationLifetime lifeTime, IKernelMemoryService kernelMemoryService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_kernelMemoryService = kernelMemoryService ?? throw new ArgumentNullException(nameof(kernelMemoryService));
		}

		[HttpPost("api/add")]
		public async Task<IActionResult> Add(IFormFile json)
		{
			if (json == null)
			{
				throw new Exception("Request json not found.");
			}

			var memory = _kernelMemoryService.Get();
			if (memory == null)
			{
				throw new Exception("Kernel Memory noet yet started. Please verify configuration.");
			}

			var request = await JsonSerializer.DeserializeAsync<DocumentAddRequest>(json.OpenReadStream(), ConfigExtensions.JsonOptions) ?? new();
			var uploads = (Request.HasFormContentType ? Request.Form?.Files.Where(upload => upload.FileName != "request.json") : null) ?? new FormFileCollection();


            try
			{
				
				foreach (var upload in uploads)
				{
					using (var stream = upload.OpenReadStream())
					{
						var id = await memory.ImportDocumentAsync(stream, upload.FileName);
					}
				}

				foreach (var url in request.WebUrls)
				{
					if (Uri.TryCreate(url, UriKind.Absolute, out _))
					{
						var id = await memory.ImportWebPageAsync(url);
					}
				}

				foreach (var path in request.FileUrls)
				{
					var id = await memory.ImportDocumentAsync(path.Replace("file://", ""));
				}

				return Ok();
			}
			catch (AggregateException ex)
			{
				return StatusCode(500, $"Internal server error: {ex}. Is embeddings model available?");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}

		[HttpGet("documents/ask")]
		public async Task<string> Ask(string question)
		{
			var memory = _kernelMemoryService.Get();

			var answer = await memory.AskAsync(question);
			return answer.Result;
		}
	}

    public class  DocumentAddRequest()
    {
		public List<string> Tags { get; set; } = new();
		public List<string> Urls { get; set; } = new();
        public List<string> WebUrls => Urls.Where(path => path.StartsWith("http")).ToList();
		public List<string> FileUrls => Urls.Where(path => path.StartsWith("file://")).ToList();
	}
}
