using Howabout.Configuration;
using Howabout.Controllers;
using Howabout.Extensions;
using Howabout.Interfaces;
using Serilog;
using System.Linq;

namespace Howabout.Commands
{
	public class ConsoleCommandAdd : IConsoleCommand
	{
		private readonly ConsoleStartupArguments _args;
		private readonly HttpMessageHandler _httpMessageHandler;

        public ConsoleCommandAdd(ConsoleStartupArguments args, HttpMessageHandler? httpMessageHandler = null)
        {
			_args = args ?? throw new ArgumentNullException(nameof(args));
			_httpMessageHandler = httpMessageHandler ?? new HttpClientHandler();
		}

        public Task Verify()
		{
			if (_args.Options.Count() == 0)
			{
				Log.Error("No options specified for a path or url to add.");
			}

			return Task.CompletedTask;
		}

		public async Task Execute()
		{
			using var client = new HttpClient(_httpMessageHandler);
			client.Timeout = TimeSpan.FromMinutes(15);

			var urls = _args.GetUrls();
			var filenames = _args.Options
				.Except(_args.GetUrls())
				.ToFullPaths()
				.IncludeDirectoryFiles();

			if (urls.Count() > 0)
			{
				await SendUrls(client, urls);
			}
			
			foreach (var filepath in filenames)
			{
				await SendFileUpload(client, filepath);
			}
		}

		private async Task SendUrls(HttpClient client, List<string> urls)
		{
			var request = new DocumentAddRequest() { Tags = _args.GetTags(), Urls = urls };
			var response = await client.PostAsJsonAsync("http://localhost:5153/api/add", request, ConfigExtensions.JsonOptions);
			if (response.IsSuccessStatusCode)
			{
				Log.Information("Urls added: {Url}", string.Join(", ", urls));
			}
			else
			{
				Log.Error("Urls failed to add: {Url} {Reason}", string.Join(", ", urls), response.ReasonPhrase);
			}
		}

		private async Task SendFileUpload(HttpClient client, string filepath)
		{
			using (var multipart = new MultipartFormDataContent())
			{
				multipart.Add(JsonContent.Create(new DocumentAddRequest() { Tags = _args.GetTags() }, options: ConfigExtensions.JsonOptions), "json", "request.json");
				using (var binaryContent = new ByteArrayContent(await File.ReadAllBytesAsync(filepath)))
				{
					binaryContent.Headers.ContentType = new("application/octet-stream");
					multipart.Add(binaryContent, Path.GetFileName(filepath), Path.GetFileName(filepath));
					var response = await client.PostAsync("http://localhost:5153/api/add", multipart);
					if (response.IsSuccessStatusCode)
					{
						Log.Information("Document added: {FileName}", filepath);
					}
					else
					{
						Log.Error("Document failed: {FileName} {Reason}", filepath, response.ReasonPhrase);
					}
				}
			}
		}
	}
}
