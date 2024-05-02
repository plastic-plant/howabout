using Azure;
using Howabout.Configuration;
using Howabout.Controllers;
using Howabout.Interfaces;
using Serilog;

namespace Howabout.Commands
{
	public class ConsoleCommandAsk: IConsoleCommand
	{
		private readonly ConsoleStartupArguments _args;
		private readonly HttpMessageHandler _httpMessageHandler;

		public ConsoleCommandAsk(ConsoleStartupArguments args, HttpMessageHandler? httpMessageHandler = null)
        {
			_args = args ?? throw new ArgumentNullException(nameof(args));
			_httpMessageHandler = httpMessageHandler ?? new HttpClientHandler();
        }

        public Task Verify()
		{
			return Task.CompletedTask;
		}

		public async Task Execute()
		{	
			using var client = new HttpClient(_httpMessageHandler);
			client.Timeout = TimeSpan.FromMinutes(15);

			var question = string.Join(" ", _args.Options);
			var request = new DocumentAskRequest { Question = question, Tags = _args.GetTags() };
			var response = await client.PostAsJsonAsync("http://localhost:5153/api/ask", request, ConfigExtensions.JsonOptions);
			if (response.IsSuccessStatusCode)
			{
				Log.Information(await response.Content.ReadAsStringAsync());
			}
			else
			{
				Log.Error("Error: {Reason}", question, response.ReasonPhrase);
			}
			return;
		}

	}
}
