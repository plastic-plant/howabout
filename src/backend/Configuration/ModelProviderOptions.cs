using Microsoft.KernelMemory;
using Microsoft.KernelMemory.Configuration;

namespace Howabout.Configuration
{
	public class ModelProviderOptions
	{
		public const string Section = "Providers";
		public PersistenceConfig Persistence { get; set; } = new();
		public PartitioningModel Partitioning { get; set; } = new();
		public CompletionsModel Completions { get; set; } = new();
		public EmbeddingsModel Embeddings { get; set; } = new();

		public class PersistenceConfig : QdrantConfig
		{
			public const string Section = $"{ModelProviderOptions.Section}:Persistence";
			public StorageProvider Provider { get; set; }
			public string Directory { get; set; } = "_vectors";

		}

		public class PartitioningModel : TextPartitioningOptions
		{
			public const string Section = $"{ModelProviderOptions.Section}:Partitioning";
		}

		public class CompletionsModel : OpenAIConfig
		{
			public const string Section = $"{ModelProviderOptions.Section}:Completions";
			public ModelProvider Provider { get; set; }
			public string ModelPath { get; set; } = "";
			public int? GpuLayerCount { get; set; }
			public uint? Seed { get; set; }
		}

		public class EmbeddingsModel : OpenAIConfig
		{
			public const string Section = $"{ModelProviderOptions.Section}:Embeddings";
			public ModelProvider Provider { get; set; }
			public string ModelPath { get; set; } = "";
			public int? GpuLayerCount { get; set; }
			public uint? Seed { get; set; }
		}

		public enum ModelProvider
		{
			None,
			File,
			OpenAI,
		}

		public enum StorageProvider
		{
			None,
			File,
			Qdrant,
		}
	}
}
