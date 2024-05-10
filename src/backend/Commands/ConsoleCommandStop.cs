using Howabout.Configuration;
using Howabout.Interfaces;
using Serilog;

namespace Howabout.Commands
{
	public class ConsoleCommandStop : IConsoleCommand
	{
		private readonly ConsoleStartupArguments _args;
		private readonly HttpMessageHandler _httpMessageHandler;

		public ConsoleCommandStop(ConsoleStartupArguments args, HttpMessageHandler? httpMessageHandler = null)
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
			using (var client = new HttpClient(_httpMessageHandler))
			{
				client.Timeout = TimeSpan.FromMinutes(15);
				Console.WriteLine(App.Settings.Url + "api/stop");
				var response = await client.GetAsync(App.Settings.Url + "api/stop");
				if (response.IsSuccessStatusCode)
				{
					Log.Information("Stopped.");
				}
				else
				{
					Log.Error("Failed to stop. {Reason}", response.ReasonPhrase);
				}
			};
			return;
		}

	}
}
