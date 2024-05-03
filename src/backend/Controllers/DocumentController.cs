using Howabout.Configuration;
using Howabout.Extensions;
using Howabout.Hubs;
using Howabout.Interfaces;
using Howabout.Models;
using Howabout.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Text.Json;

namespace Howabout.Controllers
{
	public class DocumentController : Controller
	{
		private readonly ILogger<DocumentController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IKernelMemoryService _kernelMemoryService;
		private readonly IDocumentCache _documentCache;
		private readonly IHubContext<EventMessageHub, IEventMessageClient> _eventMessageHub;
		private readonly IConversationService _conversation;


		public DocumentController(ILogger<DocumentController> logger, IConfiguration configuration, IHostApplicationLifetime lifeTime, IKernelMemoryService kernelMemoryService, IDocumentCache documentCache, IHubContext<EventMessageHub, IEventMessageClient> eventMessageHub, IConversationService conversation)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_kernelMemoryService = kernelMemoryService ?? throw new ArgumentNullException(nameof(kernelMemoryService));
			_documentCache = documentCache ?? throw new ArgumentNullException(nameof(documentCache));
			_eventMessageHub = eventMessageHub ?? throw new ArgumentNullException(nameof(eventMessageHub));
			_conversation = conversation ?? throw new ArgumentNullException(nameof(conversation));
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
				throw new Exception("Kernel Memory not yet started. Please verify configuration.");
			}

			var request = await JsonSerializer.DeserializeAsync<DocumentAddRequest>(json.OpenReadStream(), ConfigExtensions.JsonOptions) ?? new();
			var uploads = (Request.HasFormContentType ? Request.Form?.Files.Where(upload => upload.FileName != "request.json") : null) ?? new FormFileCollection();

            try
			{
				foreach (var upload in uploads)
				{
					var stopwatch = Stopwatch.StartNew();
					using (var stream = upload.OpenReadStream())
					{
						var id = await memory.ImportDocumentAsync(stream, upload.FileName);
						var document = new DocumentProperties() { Id = id, Tags = request.Tags, Name = upload.Name, Extension = Path.GetExtension(upload.FileName)?.ToLower().Replace(".", "") ?? "", OriginalPath = upload.FileName, Size = upload.Length.ToFileSizeFormatted() };
						if (_documentCache.AddOrUpdate(document))
						{
							stopwatch.Stop();
							_conversation.AddMessage(new ConversationMessage
							{
								MessageType = ConversationMessageType.DocumentChange,
								MessageData = document,
								ProcessingTimeSeconds = (int)stopwatch.Elapsed.TotalSeconds
							});
						}
					}
				}

				foreach (var url in request.WebUrls)
				{
					if (Uri.TryCreate(url, UriKind.Absolute, out _))
					{
						var stopwatch = Stopwatch.StartNew();
						var id = await memory.ImportWebPageAsync(url);
						var document = new DocumentProperties() { Id = id, Tags = request.Tags, Name = url, Extension = Path.GetExtension(url)?.ToLower().Replace(".", "") ?? "", OriginalPath = url };
						if (_documentCache.AddOrUpdate(document))
						{
							_conversation.AddMessage(new()
							{
								MessageType = ConversationMessageType.DocumentChange,
								MessageData = document,
								ProcessingTimeSeconds = (int)stopwatch.Elapsed.TotalSeconds
							});
						}
					}
				}

				foreach (var path in request.FileUrls)
				{
					var stopwatch = Stopwatch.StartNew();
					var id = await memory.ImportDocumentAsync(path.Replace("file://", ""));
					var document = new DocumentProperties() { Id = id, Tags = request.Tags, Name = path, Extension = Path.GetExtension(path)?.ToLower().Replace(".", "") ?? "", OriginalPath = path };
					if (_documentCache.AddOrUpdate(document))
					{
						_conversation.AddMessage(new()
						{
							MessageType = ConversationMessageType.DocumentChange,
							MessageData = document,
							ProcessingTimeSeconds = (int)stopwatch.Elapsed.TotalSeconds
						});
					}
				}

				await _eventMessageHub.Clients.All.DocumentChangedEvent();
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
		

		[HttpPost("api/ask")]
		public async Task<string> Ask([FromBody] DocumentAskRequest request)
		{
			_conversation.AddMessage(new()
			{
				Role = ConversationMessageRole.User,
				MessageType = ConversationMessageType.Conversation,
				MessageText = request.Question			
			});

			var stopwatch = Stopwatch.StartNew();
			var memory = _kernelMemoryService.Get();
			var answer = await memory.AskAsync(request.Question);

			_conversation.AddMessage(new()
			{
				Role = ConversationMessageRole.Assistant,
				MessageType = ConversationMessageType.Conversation,
				MessageText = answer.Result,
				MessageData = answer.RelevantSources,
				ProcessingTimeSeconds = (int)stopwatch.Elapsed.TotalSeconds
			});
			return answer.Result;
		}

		[HttpGet("api/messages")]
		public Task<List<ConversationMessage>> GetMessagesAsync()
		{
			return _conversation.GetMessagesAsync();
		}
	}

	public class DocumentAddRequest()
    {
		public List<string> Tags { get; set; } = new();
		public List<string> Urls { get; set; } = new();
        public List<string> WebUrls => Urls.Where(path => path.StartsWith("http")).ToList();
		public List<string> FileUrls => Urls.Where(path => path.StartsWith("file://")).ToList();
	}

	public class DocumentAskRequest()
	{
        public string Question { get; set; }
        public List<string> Tags { get; set; } = new();
	}
}
