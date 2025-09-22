using LoanManagement.Models;

namespace LoanManagement.Repositories.Interface
{
    public interface IDocumentRepository
    {
        Task<Document?> GetByIdAsync(int documentId);
        Task<IEnumerable<Document>> GetAllDocumentsAsync();
        Task<IEnumerable<Document>> GetDocumentsByApplicationAsync(int applicationId);
        Task<Document?> GetDocumentByFileNameAsync(string fileName);
        Task AddDocumentAsync(Document document);
        void UpdateDocument(Document document);
        void DeleteDocument(Document document);
        Task<bool> SaveAsync();
    }
}
