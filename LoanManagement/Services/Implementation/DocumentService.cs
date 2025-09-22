using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using LoanManagement.Services.Interface;

namespace LoanManagement.Services.Implementation
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepo;

        public DocumentService(IDocumentRepository documentRepo)
        {
            _documentRepo = documentRepo;
        }

        public async Task<int> UploadDocumentAsync(Document document, byte[] fileContent)
        {
            document.UploadDate = DateTime.Now;
            await _documentRepo.AddDocumentAsync(document);

            if (await _documentRepo.SaveAsync())
                return document.DocumentId;

            return 0;
        }

        public async Task<Document?> GetDocumentDetailsAsync(int documentId)
        {
            return await _documentRepo.GetByIdAsync(documentId);
        }

        public async Task<byte[]?> DownloadDocumentAsync(int documentId)
        {
            var document = await _documentRepo.GetByIdAsync(documentId);
            if (document == null) return null;

            return File.ReadAllBytes(document.FilePath);
        }

        public async Task<bool> DeleteDocumentAsync(int documentId)
        {
            var document = await _documentRepo.GetByIdAsync(documentId);
            if (document == null) return false;

            if (File.Exists(document.FilePath))
                File.Delete(document.FilePath);

            _documentRepo.DeleteDocument(document);
            return await _documentRepo.SaveAsync();
        }

        public async Task<IEnumerable<Document>> GetApplicationDocumentsAsync(int applicationId)
        {
            return await _documentRepo.GetDocumentsByApplicationAsync(applicationId);
        }

        public async Task<bool> VerifyDocumentCompletenessAsync(int applicationId)
        {
            var documents = await _documentRepo.GetDocumentsByApplicationAsync(applicationId);
            var requiredDocs = new[] { "Identity", "Address", "Income", "Bank" };

            return requiredDocs.All(rd => documents.Any(d => d.FileName.Contains(rd, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<IEnumerable<string>> GetMissingDocumentTypesAsync(int applicationId)
        {
            var documents = await _documentRepo.GetDocumentsByApplicationAsync(applicationId);
            var requiredDocs = new[] { "Identity Proof", "Address Proof", "Income Proof", "Bank Statement" };

            return requiredDocs.Where(rd => !documents.Any(d => d.FileName.Contains(rd, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<bool> ValidateDocumentAsync(int documentId)
        {
            var document = await _documentRepo.GetByIdAsync(documentId);
            return document != null && File.Exists(document.FilePath);
        }

        public async Task<bool> IsDocumentTypeRequiredAsync(string documentType, int schemeId)
        {
            var requiredDocs = new[] { "Identity Proof", "Address Proof", "Income Proof", "Bank Statement" };
            return requiredDocs.Contains(documentType);
        }
    }

}
