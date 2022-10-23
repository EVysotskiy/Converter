using Domain.Model;

namespace Domain.Services;

public interface IDocumentService
{
   Task<Document> Get(long id);
   Task<List<Document>> GetAll(long userId);
   Task Add(Document document);
}