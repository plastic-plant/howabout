using Howabout.Configuration;
using Howabout.Interfaces;
using Howabout.Models;
using Howabout.Repositories;
using HuggingfaceHub;
using LibGit2Sharp;
using Serilog;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.IO.Compression;

namespace Howabout.Commands
{
	public class ConsoleCommandDownload: IConsoleCommand
	{
		private readonly ConsoleStartupArguments _args;
		private readonly IDownloadRepository _download;

		public ConsoleCommandDownload(ConsoleStartupArguments args, IDownloadRepository? download = null)
        {
			_args = args ?? throw new ArgumentNullException(nameof(args));
			_download = download ?? new DownloadRepository(args);
			
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
				var downloadpath = await _download.DownloadAsync(model);
				Directory.CreateDirectory(_download.LocalModelDirectory);
				switch (_download.DownloadType)
				{
					// Extract downloaded .tar.gz or .zip archive.
					case DownloadType.HttpTarGz:
					case DownloadType.HttpZip:
						using (var archive = ArchiveFactory.Open(downloadpath))
						{
							foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
							{
								entry.WriteToDirectory(_download.LocalModelDirectory, new ExtractionOptions()
								{
									ExtractFullPath = true,
									Overwrite = false
								});
							}
						}
						break;

					// Do not extract downloaded model files from cloned repository.
					default:
						break;
				}
			}
		}
	}
}
