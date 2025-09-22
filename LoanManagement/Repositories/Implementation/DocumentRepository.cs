using LoanManagement.Data;
using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Repositories.Implementation
{
    /// Document Repository - File management system
    /// Loan application documents ka storage aur retrieval
    /// </summary>
    public class DocumentRepository : IDocumentRepository
    {
        private readonly AppDbContext _context;

        public DocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Document?> GetByIdAsync(int documentId)
        {
            return await _context.Documents.FindAsync(documentId);
        }

        /// <summary>
        /// All documents - Document management dashboard
        /// </summary>
        public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            return await _context.Documents
                .Include(d => d.LoanApplication)
                    .ThenInclude(la => la.Customer)
                        .ThenInclude(c => c.User)
                .OrderByDescending(d => d.UploadDate)  // Latest uploads first
                .ToListAsync();
        }

        /// <summary>
        /// Application specific documents - Document verification ke liye
        /// Loan officer ko application review karte time saare docs chahiye
        /// </summary>
        public async Task<IEnumerable<Document>> GetDocumentsByApplicationAsync(int applicationId)
        {
            return await _context.Documents
                .Where(d => d.ApplicationId == applicationId)
                .OrderByDescending(d => d.UploadDate)
                .ToListAsync();
        }

        /// <summary>
        /// File search - Specific document find karne ke liye
        /// </summary>
        public async Task<Document?> GetDocumentByFileNameAsync(string fileName)
        {
            return await _context.Documents
                .Include(d => d.LoanApplication)
                .FirstOrDefaultAsync(d => d.FileName == fileName);
        }

        public async Task AddDocumentAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
        }

        public void UpdateDocument(Document document)
        {
            _context.Documents.Update(document);
        }

        public void DeleteDocument(Document document)
        {
            _context.Documents.Remove(document);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
