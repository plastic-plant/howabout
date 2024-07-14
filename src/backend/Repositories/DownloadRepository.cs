using Elastic.Clients.Elasticsearch;
using Howabout.Configuration;
using Howabout.Models;
using HuggingfaceHub;
using LibGit2Sharp;
using Serilog;
using System.Net;

namespace Howabout.Repositories
{
	public class DownloadRepository: IDownloadRepository
	{
		public string ModelUrl { get; set; } = "";
		public DownloadType DownloadType { get; set; }
		public string LocalModelsDirectory { get; set; } = Path.Combine(Program.ApplicationRootDirectory, "_models");
		public string LocalModelDirectory { get; set; } = ""; // C:\Program Files\Howabout\_models\google\gemma-2b-it
		public string? Username { get; set; }
		public string? Password { get; set; }
        public bool IsGivenCredentials => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

		public DownloadRepository(ConsoleStartupArguments? args = null)
        {
			if (args != null)
			{
				var lookup = args.Parameters.ToLookup(kvp => kvp.Key, kvp => kvp.Value);
				Username = lookup["--username"].FirstOrDefault() ?? lookup["-u"].FirstOrDefault();
				Password = lookup["--password"].FirstOrDefault() ?? lookup["-p"].FirstOrDefault();
			}
		}

		public async Task<string> DownloadAsync(string? url = null)
		{
			ModelUrl = url ?? ModelUrl;
			DownloadType = ModelUrl switch
			{
				_ when ModelUrl.StartsWith("http") && ModelUrl.EndsWith(".zip") => DownloadType.HttpZip,
				_ when ModelUrl.StartsWith("http") && ModelUrl.EndsWith(".tar.gz") => DownloadType.HttpTarGz,
				_ when ModelUrl.StartsWith("https://") && ModelUrl.EndsWith(".git") => DownloadType.GitRepo,
				_ when ModelUrl.Contains("/") => DownloadType.HuggingFaceRepo,
				_ => DownloadType.Unknown
			};

			Log.Information($"Downloading {DownloadType} model from {ModelUrl} to {LocalModelsDirectory} folder.");
			switch (DownloadType)
			{
				// howabout download google/gemma-2b-it
				case DownloadType.HuggingFaceRepo:
					LocalModelDirectory = Path.Combine(LocalModelsDirectory, ModelUrl);
					Directory.CreateDirectory(ModelUrl);
					await HFDownloader.DownloadSnapshotAsync(
						ModelUrl,
						"main",
						localDir: LocalModelDirectory,
						maxWorkers: 4,
						progress: new DownloadProgress());
					return LocalModelDirectory;

				// howabout download https://github.com/plastic-plant/fineprint.git -u sixfingers -p 123456!
				case DownloadType.GitRepo:
					var options = new CloneOptions();
					if (IsGivenCredentials)
					{
						options.FetchOptions.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = Username, Password = Password };
					}
					var relative = (new Uri(ModelUrl)).Query.Trim('/').Replace(".git", ""); // i.e. "plastic-plant/fineprint"
					LocalModelDirectory = Path.Combine(LocalModelsDirectory, relative);
					Repository.Clone(ModelUrl, LocalModelDirectory, options);
					return LocalModelDirectory;

				// howabout download http://localhost/models/example.zip
				// howabout download https://github.com/patrickdekleijn/sixfingers/models/example.tar.gz
				case DownloadType.HttpZip:
				case DownloadType.HttpTarGz:
					LocalModelDirectory = Path.Combine(LocalModelsDirectory, Path.GetFileNameWithoutExtension(ModelUrl));
					return await DownloadHttpAsync(ModelUrl);

				case DownloadType.Unknown:
				default:
					throw new ArgumentException("Model url to download not understood. Please type a url or repository and model, e.g. `howabout download google/gemma-2b-it`"); ;
			}
		}

		private async Task<string> DownloadHttpAsync(string modelUrl)
		{
			using (var client = new WebClient())
			{
				if (IsGivenCredentials)
				{
					client.Credentials = new NetworkCredential(Username, Password);
				}
				var filename = Path.GetFileName(modelUrl); // i.e. http://localhost/models/example.zip -> "example.zip"
				var filepath = Path.Combine(LocalModelsDirectory, filename);
				await client.DownloadFileTaskAsync(modelUrl, filepath);
				return filepath;
			}
		}

		private class DownloadProgress : IGroupedProgress
		{
			public void Report(string filename, int progress)
			{
				Log.Information(filename + " " + progress + "%");
			}
		}
	}
}
