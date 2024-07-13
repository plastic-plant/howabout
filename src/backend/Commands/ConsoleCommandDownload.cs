using Azure;
using Howabout.Configuration;
using Howabout.Controllers;
using Howabout.Interfaces;
using HuggingfaceHub;
using LibGit2Sharp;
using Serilog;

namespace Howabout.Commands
{
	public class ConsoleCommandDownload: IConsoleCommand
	{
		private readonly ConsoleStartupArguments _args;
		private readonly HttpMessageHandler _httpMessageHandler;
		private readonly string _modelsDownloadPath;

		public ConsoleCommandDownload(ConsoleStartupArguments args, HttpMessageHandler? httpMessageHandler = null)
        {
			_args = args ?? throw new ArgumentNullException(nameof(args));
			_httpMessageHandler = httpMessageHandler ?? new HttpClientHandler();
			_modelsDownloadPath = Path.Combine(Program.ApplicationRootDirectory, "_models");
		}

        public Task Verify()
		{
			if (_args.Arguments.Count == 0)
			{
				throw new ArgumentException("Model to download not given. Please type a repository and model, e.g. `howabout download google/gemma-2b-it`");
			}
			return Task.CompletedTask;
		}

		public async Task Execute()
		{
			foreach (var model in _args.Arguments)
			{
				var isGitRepo = model.StartsWith("https://");
				if (isGitRepo)
				{
					var repoModelPath = (new Uri(model)).Query.Trim('/').Replace(".git", ""); // e.g. https://github.com/plastic-plant/fineprint -> "plastic-plant/fineprint"
					var localModelDirectory = Path.Combine(_modelsDownloadPath, repoModelPath);
					Repository.Clone("https://github.com/libgit2/libgit2sharp.git", localModelDirectory);
				}
				else // Download from Hugging Face.
				{
					var repoModelPath = model; // e.g. "google/gemma-2b-it"
					var localModelDirectory = Path.Combine(_modelsDownloadPath, repoModelPath);
					if (!Directory.Exists(localModelDirectory))
					{
						Directory.CreateDirectory(localModelDirectory);
					}
					await HFDownloader.DownloadSnapshotAsync(model, "main", localDir: localModelDirectory, maxWorkers: 4, progress: new DownloadFilesGroupProgress());
				}
			}
			return;
		}

		private class DownloadFilesGroupProgress : IGroupedProgress
		{
			public void Report(string filename, int progress)
			{
				Log.Information(filename + " " + progress + "%");
			}
		}
	}
}
