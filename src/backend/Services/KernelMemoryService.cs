using Howabout.Configuration;
using Howabout.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;

namespace Howabout.Services
{
	public class KernelMemoryService : IKernelMemoryService
	{
		private readonly ModelProviderOptions _options;
		private volatile IKernelMemory? _kernelMemory;

        public KernelMemoryService(IOptions<ModelProviderOptions> options)
        {
			_options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            
        }

		public IKernelMemory? Get() => _kernelMemory;

		public void Configure()
		{
			_kernelMemory = new KernelMemoryBuilder()
				.WithOpenAITextGeneration(_options.Completions)
				.WithOpenAITextEmbeddingGeneration(_options.Embeddings)
				.WithCustomTextPartitioningOptions(new()
				{
					MaxTokensPerParagraph = _options.Embeddings.EmbeddingModelMaxTokenTotal, // Can do 8K, but 512 is LM Studio limit embeddings for vram for now
					MaxTokensPerLine = _options.Embeddings.EmbeddingModelMaxTokenTotal,
					OverlappingTokens = _options.Partitioning.OverlappingTokens,
				})
				.Build();
		}
	}
}
