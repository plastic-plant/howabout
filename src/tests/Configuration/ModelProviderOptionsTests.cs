using Howabout.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Howabout.Configuration.ModelProviderOptions;

namespace Howabout.Tests.Configuration
{
	public class ModelProviderOptionsTests
	{
		private const string _appSettingsProvidersDefault = @"
        {
          ""Providers"": {
            ""Partitioning"": {
            },
            ""Persistence"": {
            },
            ""Embeddings"": {
            },
            ""Completions"": {
            }
          }
        }";

		private const string _appSettingsProvidersCustom = @"
        {
          ""Providers"": {
            ""Partitioning"": {
              ""MaxTokensPerParagraph"": 1000,
              ""MaxTokensPerLine"": 300,
              ""OverlappingTokens"": 100
            },
            ""Persistence"": {
              ""Storage"": ""Qdrant"",
              ""Directory"": ""_tmp"",
              ""Endpoint"": ""http://localhost:6333"",
              ""APIKey"": ""None""
            },
            ""Embeddings"": {
              ""Endpoint"": ""http://localhost:1234/v1/"",
              ""EmbeddingModel"": ""lm-studio"",
              ""EmbeddingModelMaxTokenTotal"": 2048,
              ""APIKey"": ""None"",
              ""OrgId"": ""org-aBCd"",
              ""MaxRetries"": 12
            },
            ""Completions"": {
              ""Endpoint"": ""http://localhost:1234/v1/"",
              ""TextModel"": ""lm-studio"",
              ""TextModelMaxTokenTotal"": 32768,
              ""APIKey"": ""None"",
              ""OrgId"": ""org-aBCd"",
              ""MaxRetries"": 2
            }
          }
        }";

		[Fact]
		public void ModelProviderOptions_WithoutConfiguration_ReturnsDefaults()
		{
			var configuration = new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(_appSettingsProvidersDefault)))
				.Build();

			var serviceProvider = new ServiceCollection()
				.AddOptions()
				.Configure<ModelProviderOptions>(configuration.GetSection(ModelProviderOptions.Section))
				.BuildServiceProvider();

            var options = serviceProvider.GetRequiredService<IOptions<ModelProviderOptions>>().Value;

            Assert.Equal(1000, options.Partitioning.MaxTokensPerParagraph);
			Assert.Equal(300, options.Partitioning.MaxTokensPerLine);
			Assert.Equal(100, options.Partitioning.OverlappingTokens);

			Assert.Equal(StorageProviders.None, options.Persistence.Storage);
			Assert.Equal("_tmp-memory-vectors", options.Persistence.Directory);
			Assert.Equal(string.Empty, options.Persistence.Endpoint);
			Assert.Equal(string.Empty, options.Persistence.APIKey);

			Assert.Equal("https://api.openai.com/v1", options.Embeddings.Endpoint);
			Assert.Equal(string.Empty, options.Embeddings.EmbeddingModel);
			Assert.Equal(8191, options.Embeddings.EmbeddingModelMaxTokenTotal);
			Assert.Equal(string.Empty, options.Embeddings.APIKey);
			Assert.Equal(string.Empty, options.Embeddings.OrgId);
			Assert.Equal(10, options.Embeddings.MaxRetries);

			Assert.Equal("https://api.openai.com/v1", options.Completions.Endpoint);
			Assert.Equal(string.Empty, options.Completions.TextModel);
			Assert.Equal(8192, options.Completions.TextModelMaxTokenTotal);
			Assert.Equal(string.Empty, options.Completions.APIKey);
			Assert.Equal(string.Empty, options.Completions.OrgId);
			Assert.Equal(10, options.Completions.MaxRetries);
		}

		[Fact]
		public void ModelProviderOptions_WithConfiguration_ReturnsCustom()
		{
			var configuration = new ConfigurationBuilder()
				.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(_appSettingsProvidersCustom)))
				.Build();

			var serviceProvider = new ServiceCollection()
				.AddOptions()
				.Configure<ModelProviderOptions>(configuration.GetSection(ModelProviderOptions.Section))
				.BuildServiceProvider();

			var options = serviceProvider.GetRequiredService<IOptions<ModelProviderOptions>>().Value;

			Assert.Equal(1000, options.Partitioning.MaxTokensPerParagraph);
			Assert.Equal(300, options.Partitioning.MaxTokensPerLine);
			Assert.Equal(100, options.Partitioning.OverlappingTokens);

			Assert.Equal(StorageProviders.Qdrant, options.Persistence.Storage);
			Assert.Equal("_tmp", options.Persistence.Directory);
			Assert.Equal("http://localhost:6333", options.Persistence.Endpoint);
			Assert.Equal("None", options.Persistence.APIKey);

			Assert.Equal("http://localhost:1234/v1/", options.Embeddings.Endpoint);
			Assert.Equal("lm-studio", options.Embeddings.EmbeddingModel);
			Assert.Equal(2048, options.Embeddings.EmbeddingModelMaxTokenTotal);
			Assert.Equal("None", options.Embeddings.APIKey);
			Assert.Equal("org-aBCd", options.Embeddings.OrgId);
			Assert.Equal(12, options.Embeddings.MaxRetries);

			Assert.Equal("http://localhost:1234/v1/", options.Completions.Endpoint);
			Assert.Equal("lm-studio", options.Completions.TextModel);
			Assert.Equal(32768, options.Completions.TextModelMaxTokenTotal);
			Assert.Equal("None", options.Completions.APIKey);
			Assert.Equal("org-aBCd", options.Completions.OrgId);
			Assert.Equal(2, options.Completions.MaxRetries);
		}

	}
}
