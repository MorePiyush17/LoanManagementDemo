using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    /// Document Service Interface - File management system
    /// Document upload, verification, storage management
    /// </summary>
    public interface IDocumentService
    {
        Task<int> UploadDocumentAsync(Document document, byte[] fileContent);
        Task<Document?> GetDocumentDetailsAsync(int documentId);
        Task<byte[]?> DownloadDocumentAsync(int documentId);
        Task<bool> DeleteDocumentAsync(int documentId);
        Task<IEnumerable<Document>> GetApplicationDocumentsAsync(int applicationId);
        Task<bool> VerifyDocumentCompletenessAsync(int applicationId);
        Task<IEnumerable<string>> GetMissingDocumentTypesAsync(int applicationId);
        Task<bool> ValidateDocumentAsync(int documentId);
        Task<bool> IsDocumentTypeRequiredAsync(string documentType, int schemeId);
    }
}