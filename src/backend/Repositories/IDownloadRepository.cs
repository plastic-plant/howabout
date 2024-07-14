using Howabout.Models;

namespace Howabout.Repositories
{
    public interface IDownloadRepository
    {
		/// <summary>
		/// Model given as a URL or repository/model for Hugging Face, as requested for download:
		/// 
		/// howabout download google/gemma-2b-it
		/// howabout download https://github.com/plastic-plant/fineprint.git -u sixfingers -p 123456!
		/// howabout download http://localhost/models/example.zip
		/// howabout download https://github.com/patrickdekleijn/sixfingers/models/example.tar.gz
		/// 
		/// </summary>
		string ModelUrl { get; set; }
		DownloadType DownloadType { get; set; }

		/// <summary>
		/// Full path to _models directory under howabout program root directory.
		/// </summary>
        string LocalModelsDirectory { get; set; }

		/// <summary>
		/// Full path to _models/repository/model directory for this specific model.
		/// </summary>
		string LocalModelDirectory { get; set; }

		/// <summary>
		/// Optional username for downloading from private repositories.
		/// </summary>
		string? Username { get; set; }

		/// <summary>
		/// Optional password for downloading from private repositories.
		/// </summary>
		string? Password { get; set; }

		Task<string> DownloadAsync(string? url = null);

	}
}
