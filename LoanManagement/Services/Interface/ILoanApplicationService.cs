using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    public interface ILoanApplicationService
    {
        Task<int> SubmitApplicationAsync(LoanApplication application);
        Task<bool> UploadDocumentsAsync(int applicationId, IEnumerable<Document> documents);
        Task<LoanApplication?> GetApplicationDetailsAsync(int applicationId);
        Task<IEnumerable<LoanApplication>> GetPendingApplicationsAsync();
        Task<IEnumerable<LoanApplication>> GetApplicationsByStatusAsync(string status);
        Task<bool> AssignApplicationToOfficerAsync(int applicationId, int officerId);
        Task<bool> UpdateApplicationStatusAsync(int applicationId, string status, string remarks);
        Task<bool> ValidateApplicationEligibilityAsync(LoanApplication application);
        Task<bool> VerifyDocumentCompletenessAsync(int applicationId);
        Task<IEnumerable<string>> GetMissingDocumentsAsync(int applicationId);
        Task<IEnumerable<LoanApplication>> GetCustomerApplicationsAsync(int customerId);
    }
}