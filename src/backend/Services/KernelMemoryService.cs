using Howabout.Configuration;
using Howabout.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;
using Serilog;

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

		public async Task<bool> IsReadyAsync()
		{
			try
			{
				var indexes = await _kernelMemory?.ListIndexesAsync();
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
				.WithOpenAITextGeneration(_options.Completions)
				.WithOpenAITextEmbeddingGeneration(_options.Embeddings)
				.WithCustomTextPartitioningOptions(_options.Partitioning);

			builder.Services.AddLogging(builder =>
			{
				builder.AddConfiguration(App.Settings.Configuration);
			});

			switch (_options.Persistence.Storage)
			{
				case ModelProviderOptions.StorageProviders.File:
					builder.WithSimpleVectorDb(new SimpleVectorDbConfig()
					{
						StorageType = FileSystemTypes.Disk,
						Directory = Path.IsPathRooted(_options.Persistence.Directory) ? _options.Persistence.Directory : Path.Combine(Program.ApplicationRootDirectory, _options.Persistence.Directory),
					});
					break;

				case ModelProviderOptions.StorageProviders.Qdrant:
					builder.WithQdrantMemoryDb(_options.Persistence);
					break;

				case ModelProviderOptions.StorageProviders.None:
				default:
					builder.WithSimpleVectorDb(SimpleVectorDbConfig.Volatile);
					break;
			}

			_kernelMemory = builder.Build();
		}
	}
}
