using Howabout.Configuration;
using Howabout.Interfaces;
using Howabout.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Howabout.Controllers
{
	public class DocumentController : Controller
	{
		private readonly ILogger<DocumentController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IKernelMemoryService _kernelMemoryService;
		private readonly IDocumentCache _documentCache;

		public DocumentController(ILogger<DocumentController> logger, IConfiguration configuration, IHostApplicationLifetime lifeTime, IKernelMemoryService kernelMemoryService, IDocumentCache documentCache)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_kernelMemoryService = kernelMemoryService ?? throw new ArgumentNullException(nameof(kernelMemoryService));
			_documentCache = documentCache ?? throw new ArgumentNullException(nameof(documentCache));
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
						_documentCache.AddOrUpdate(new() { Id = id, Tags = request.Tags, Name = upload.Name, OriginalPath = upload.FileName });
					}
				}

				foreach (var url in request.WebUrls)
				{
					if (Uri.TryCreate(url, UriKind.Absolute, out _))
					{
						var id = await memory.ImportWebPageAsync(url);
						_documentCache.AddOrUpdate(new() { Id = id, Tags = request.Tags, OriginalPath = url });
					}
				}

				foreach (var path in request.FileUrls)
				{
					var id = await memory.ImportDocumentAsync(path.Replace("file://", ""));
					_documentCache.AddOrUpdate(new() { Id = id, Tags = request.Tags, Name = path, OriginalPath = path });
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

		[HttpPost("api/list")]
		public ICollection<DocumentProperties> List(string? id = null, List<string> tags = null)
		{
			return _documentCache.ListByMatchingProperties(new() { Id = id, Tags = tags ?? new List<string>() });
		}

		[HttpGet("api/listbytag/{tag}")]
		public ICollection<DocumentProperties> ListByTag(string tag) =>
			_documentCache.ListByMatchingProperties(new() { Tags = new() { tag } });

		[HttpGet("api/listgroupedbytag")]
		public Dictionary<string, List<DocumentProperties>> ListGroupedByTag()
		{
			var documents = _documentCache.List();
			var groupedbytag = new Dictionary<string, List<DocumentProperties>>();
			foreach (var document in documents)
			{
				var hasTag = document.Tags.Count() > 0;
                if (!hasTag)
                {
					document.Tags.Add("default");
                }

                foreach (var tag in document.Tags)
				{
					if (!groupedbytag.ContainsKey(tag))
					{
						groupedbytag[tag] = new List<DocumentProperties>();
					}
					groupedbytag[tag].Add(document);
				}
			}
			return groupedbytag;
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
