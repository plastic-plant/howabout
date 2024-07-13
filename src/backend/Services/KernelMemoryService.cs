using Howabout.Configuration;
using Howabout.Interfaces;
using Howabout.Repositories;
using LLama.Common;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;

namespace Howabout.Services
{
	public class KernelMemoryService(IOptions<ModelProviderOptions> options, ILoggerFactory loggerFactory) : IKernelMemoryService
	{
		private readonly ModelProviderOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
		private readonly ILoggerFactory _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
		private volatile IKernelMemory? _kernelMemory;

		public IKernelMemory? Get() => _kernelMemory;

		public async Task<bool> IsReadyAsync()
		{
			try
			{
				var indexes = await _kernelMemory!.ListIndexesAsync();
				return indexes?.Count() != null;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void Configure()
		{
			var builder = new KernelMemoryBuilder()
				.WithCustomTextPartitioningOptions(_options.Partitioning);

			builder.Services.AddLogging(builder =>
			{
				builder.AddConfiguration(App.Settings.Configuration);
			});

			switch (_options.Persistence.Provider)
			{
				case ModelProviderOptions.StorageProvider.File:
					builder.WithSimpleVectorDb(new SimpleVectorDbConfig()
					{
						StorageType = FileSystemTypes.Disk,
						Directory = Path.IsPathRooted(_options.Persistence.Directory) ? _options.Persistence.Directory : Path.Combine(Program.ApplicationRootDirectory, _options.Persistence.Directory),
					});
					break;

				case ModelProviderOptions.StorageProvider.Qdrant:
					builder.WithQdrantMemoryDb(_options.Persistence);
					break;

				case ModelProviderOptions.StorageProvider.None:
				default:
					builder.WithSimpleVectorDb(SimpleVectorDbConfig.Volatile);
					break;
			}

			switch (_options.Embeddings.Provider)
			{
				case ModelProviderOptions.ModelProvider.File:
					var modelPath = Path.IsPathRooted(_options.Embeddings.ModelPath) ? _options.Embeddings.ModelPath : Path.Combine(Program.ApplicationRootDirectory, _options.Embeddings.ModelPath);
					builder.WithCustomEmbeddingGenerator(new LocalEmbeddingGenerator(new ModelParams(modelPath)
					{
						ContextSize = (uint)_options.Embeddings.TextModelMaxTokenTotal,
						Embeddings = true,
						GpuLayerCount = _options.Completions.GpuLayerCount.HasValue ? _options.Completions.GpuLayerCount.Value : 20,
						Seed = _options.Completions.Seed
					}, _loggerFactory.CreateLogger<LocalEmbeddingGenerator>()));
					//builder.WithCustomEmbeddingGenerator(new TextEmbeddingGenerator(new()
					//{
					//	ModelPath = Path.IsPathRooted(_options.Embeddings.ModelPath) ? _options.Embeddings.ModelPath : Path.Combine(Program.ApplicationRootDirectory, _options.Embeddings.ModelPath),
					//	MaxTokenTotal = (uint)_options.Embeddings.TextModelMaxTokenTotal,
					//	Seed = _options.Completions.Seed,
					//	GpuLayerCount = _options.Completions.GpuLayerCount,
					//}, _loggerFactory.CreateLogger<TextEmbeddingGenerator>()));
					break;

				case ModelProviderOptions.ModelProvider.OpenAI:
					builder.WithOpenAITextEmbeddingGeneration(_options.Embeddings);
					break;

				case ModelProviderOptions.ModelProvider.None:
				default:
					break;
			}

			switch (_options.Completions.Provider)
			{
				case ModelProviderOptions.ModelProvider.File:
					builder.WithLlamaTextGeneration(new()
					{
						ModelPath = Path.IsPathRooted(_options.Completions.ModelPath) ? _options.Completions.ModelPath : Path.Combine(Program.ApplicationRootDirectory, _options.Completions.ModelPath),
						MaxTokenTotal = (uint)_options.Completions.TextModelMaxTokenTotal,
						Seed = _options.Completions.Seed,
						GpuLayerCount = _options.Completions.GpuLayerCount,
					});
					break;

				case ModelProviderOptions.ModelProvider.OpenAI:
					builder.WithOpenAITextGeneration(_options.Completions);
					break;

				case ModelProviderOptions.ModelProvider.None:
				default:
					break;
			}

			_kernelMemory = builder.Build();
		}
	}
}
