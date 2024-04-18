using Howabout.Interfaces;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.Configuration;

namespace Howabout.Services
{
	public class KernelMemoryService : IKernelMemoryService
	{
		private volatile IKernelMemory? _kernelMemory;

		public IKernelMemory? Get() => _kernelMemory;

		public void Configure()
		{
			var embeddingConfig = new OpenAIConfig
			{
				Endpoint = "http://localhost:1234/v1/",
				EmbeddingModel = "nomic-embed-text-v1.5.Q4_0.gguf",
				EmbeddingModelMaxTokenTotal = 512, // offload to vram
				APIKey = "lm-studio"
			};

			var generationConfig = new OpenAIConfig
			{
				Endpoint = "http://localhost:1234/v1/",
				TextModel = "gemma-2b-it",
				TextModelMaxTokenTotal = 8192,
				APIKey = "lm-studio"
			};

			var memory = new KernelMemoryBuilder()
			.WithOpenAITextEmbeddingGeneration(embeddingConfig)
			.WithOpenAITextGeneration(generationConfig)
			.WithCustomTextPartitioningOptions(new TextPartitioningOptions
			{
				MaxTokensPerParagraph = 512,
				MaxTokensPerLine = 512,
				OverlappingTokens = 50
			})
			.Build();

			_kernelMemory = memory;
		}
	}
}
