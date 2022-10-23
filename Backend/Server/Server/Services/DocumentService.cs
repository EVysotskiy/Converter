using Domain.Model;
using Domain.Services;
using Server.Repositories;

namespace Server.Services;

public class DocumentService : IDocumentService
{
    private readonly DocumentRepository _documentRepository;

    public DocumentService(DocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<Document> Get(long documentId)
    {
        return  await _documentRepository.First(x => x.Id == documentId);
    }

    public async Task<List<Document>> GetAll(long userId)
    {
        var result = await _documentRepository.Where(x => x.UserId == userId);
        return result.ToList();
    }

    public async Task Add(Document document)
    {
       await _documentRepository.Add(document);
    }
}