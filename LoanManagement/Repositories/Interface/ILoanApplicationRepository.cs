using LoanManagement.Models;

namespace LoanManagement.Repositories.Interface
{
    public interface ILoanApplicationRepository
    {
        Task<LoanApplication?> GetByIdAsync(int applicationId);
        Task<IEnumerable<LoanApplication>> GetAllApplicationsAsync();
        Task<LoanApplication?> GetApplicationWithDetailsAsync(int applicationId);
        Task<IEnumerable<LoanApplication>> GetApplicationsByCustomerAsync(int customerId);
        Task<IEnumerable<LoanApplication>> GetApplicationsByOfficerAsync(int officerId);
        Task<IEnumerable<LoanApplication>> GetApplicationsByStatusAsync(string status);
        Task<IEnumerable<LoanApplication>> GetPendingApplicationsAsync();
        Task<IEnumerable<LoanApplication>> GetApplicationsInDateRangeAsync(DateTime startDate, DateTime endDate);
        Task AddApplicationAsync(LoanApplication application);
        void UpdateApplication(LoanApplication application);
        void DeleteApplication(LoanApplication application);
        Task<bool> SaveAsync();
    }
}
