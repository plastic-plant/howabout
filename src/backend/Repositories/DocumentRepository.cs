using Howabout.Interfaces;
using System.Collections.Concurrent;

namespace Howabout.Repositories
{
	public class DocumentRepository : IDocumentCache
	{
		private ConcurrentDictionary<string, DocumentProperties> _documents { get; set; } = new();

		public bool HasDocuments() => _documents.Count > 0;

		public bool AddOrUpdate(DocumentProperties document)
		{
			if (string.IsNullOrWhiteSpace(document.Id))
			{
				return false;
			}
			else
			{
				_documents.AddOrUpdate(document.Id, document, (key, oldValue) => document);
				return true;
			}
		}

		public DocumentProperties? GetById(string id)
		{
			if (_documents.TryGetValue(id, out var document))
			{
				return document;
			}
			else
			{
				return null;
			}
		}
		public ICollection<DocumentProperties> List()
		{
			return _documents.Values;
		}


		public ICollection<DocumentProperties> ListByMatchingProperties(DocumentProperties properties)
		{
			if (!string.IsNullOrWhiteSpace(properties.Id))
			{
				var list = new List<DocumentProperties>();
				var id = GetById(properties.Id);
				if (id != null)
				{
					list.Add(id);
				}
				return list;
			}

			return _documents.Values.Where(document => document.OriginalPath == properties.OriginalPath).ToList() ?? [];
		}

		public bool Remove(string id)
		{
			return _documents.TryRemove(id, out _);
		}
	}

	public class DocumentProperties
	{
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string Extension { get; set; } = "";
        public string? OriginalPath { get; set; }
		public List<string> Tags { get; set; } = [];
		public string Size { get; set; } = "";
    }
}
