using DocumentFormat.OpenXml.Wordprocessing;
using Howabout.Repositories;

namespace Howabout.Interfaces
{
	public interface IDocumentCache
	{
		DocumentProperties? GetById(string id);
		ICollection<DocumentProperties> List();
		ICollection<DocumentProperties> ListByMatchingProperties(DocumentProperties properties);
		bool AddOrUpdate(DocumentProperties document);
		bool Remove(string id);
	}
}
