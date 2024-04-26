﻿using Howabout.Configuration;
using Howabout.Services;
using Microsoft.Extensions.Options;

namespace Howabout.Tests.Services
{
	public class KernelMemoryServiceTests
	{
		[Fact]
		public void Configure_BuildsAndThrows()
		{
			var options = new ModelProviderOptions();
			options.Completions.EmbeddingModel = "text-embedding-ada-002";
			options.Completions.APIKey = "any";
			options.Embeddings.TextModel = "gpt-3.5-turbo";
			options.Embeddings.APIKey = "any";
			var sut = new KernelMemoryService(Options.Create(options));

			var actual = () => sut.Configure();

			var exception = Assert.Throws<ArgumentException>(actual);
			Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'modelId')", exception.Message);
		}
	}
}