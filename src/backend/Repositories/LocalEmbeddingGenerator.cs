using LLama.Common;
using LLama;
using Microsoft.KernelMemory.AI;
using Microsoft.KernelMemory;

namespace Howabout.Repositories
{
	public class LocalEmbeddingGenerator : ITextEmbeddingGenerator, IDisposable
	{
		private readonly LLamaContext _context;
		private readonly LLamaEmbedder _embedder;
		private readonly LLamaWeights _weights;

		public int MaxTokens { get; set; }

		public LocalEmbeddingGenerator(ModelParams parameters, ILogger<LocalEmbeddingGenerator>? logger)
		{
			_weights = LLamaWeights.LoadFromFile(parameters);
			_embedder = new LLamaEmbedder(_weights, parameters, logger);
			_context = _weights.CreateContext(parameters, logger);
			MaxTokens = (int)(parameters.ContextSize ?? _context.ContextSize);
		}

		[Obsolete("Use GetTokens instead")]
		public int CountTokens(string text)
		{
			return _context.Tokenize(text).Length;
		}

		public IReadOnlyList<string> GetTokens(string text)
		{
			var tokens = _context.Tokenize(text);
			return tokens.Select(token => token.ToString()).ToArray();
		}

		public async Task<Embedding> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken)
		{
			var embeddings = await _embedder.GetEmbeddings(text);
			return new Embedding(embeddings);
		}

		public void Dispose()
		{
			_context.Dispose();
			_embedder.Dispose();
			_weights.Dispose();
		}
	}
}
