using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    public interface ILoanOfficerService
    {
        Task<LoanOfficer?> GetOfficerByIdAsync(int officerId);
        Task<LoanOfficer?> GetOfficerByUserIdAsync(int userId);
        Task<IEnumerable<LoanOfficer>> GetAllOfficersAsync();
        Task<bool> CreateOfficerAsync(LoanOfficer officer);
        Task<bool> UpdateOfficerAsync(LoanOfficer officer);
        Task<IEnumerable<LoanApplication>> GetOfficerWorkqueueAsync(int officerId);
        Task<int> GetOfficerWorkloadAsync(int officerId);
        Task<LoanOfficer?> GetLeastBusyOfficerInCityAsync(string city);
        Task<bool> ProcessLoanApplicationAsync(int applicationId, string decision, string remarks);
        Task<bool> RequestAdditionalDocumentsAsync(int applicationId, string documentList);
        Task<LoanApplication?> GetApplicationForReviewAsync(int applicationId);
    }
}