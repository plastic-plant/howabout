using Microsoft.KernelMemory;
using Microsoft.KernelMemory.Configuration;

namespace Howabout.Configuration
{
	public class ModelProviderOptions
	{
		public const string Section = "Providers";
		public CompletionsModel Completions { get; set; } = new();
		public EmbeddingsModel Embeddings { get; set; } = new();
		public PartitioningModel Partitioning { get; set; } = new();
        public PersistenceConfig Persistence { get; set; } = new();

		public class CompletionsModel : OpenAIConfig
		{
			public const string Section = $"{ModelProviderOptions.Section}:Completions";
		}

		public class EmbeddingsModel : OpenAIConfig
		{
			public const string Section = $"{ModelProviderOptions.Section}:Embeddings";
		}

		public class PartitioningModel : TextPartitioningOptions
		{
			public const string Section = $"{ModelProviderOptions.Section}:Partitioning";
		}

		public class PersistenceConfig : QdrantConfig
		{
			public const string Section = $"{ModelProviderOptions.Section}:Persistence";
            public StorageProviders Storage { get; set; }
			public string Directory { get; set; } = "_vectors";

		}

		public enum StorageProviders
		{
			None,
			File,
			Qdrant,
		}
	}
}
