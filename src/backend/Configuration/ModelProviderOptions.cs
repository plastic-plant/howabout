using Microsoft.KernelMemory;
using Microsoft.KernelMemory.Configuration;

namespace Howabout.Configuration
{
	public class ModelProviderOptions
	{
		public const string Section = "Providers";
		public CompletionsModel Completions { get; set; } = new();
		public EmbeddingsModel Embeddings { get; set; } = new();
		public TextPartitioningOptions Partitioning { get; set; } = new();

        public class CompletionsModel : OpenAIConfig
		{
			public const string Section = $"{ModelProviderOptions.Section}:Completions";
		}

		public class EmbeddingsModel : OpenAIConfig
		{
			public const string Section = $"{ModelProviderOptions.Section}:Embeddings";
		}
	}
}
