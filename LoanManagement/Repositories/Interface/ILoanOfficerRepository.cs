using LoanManagement.Models;

namespace LoanManagement.Repositories.Interface
{
    public interface ILoanOfficerRepository
    {
        Task<LoanOfficer?> GetByIdAsync(int officerId);
        Task<LoanOfficer?> GetByUserIdAsync(int userId);
        Task<IEnumerable<LoanOfficer>> GetAllOfficersAsync();
        Task<IEnumerable<LoanOfficer>> GetOfficersByCityAsync(string city);
        Task<LoanOfficer?> GetOfficerWithApplicationsAsync(int officerId);
        Task<int> GetApplicationCountAsync(int officerId);
        Task AddOfficerAsync(LoanOfficer officer);
        void UpdateOfficer(LoanOfficer officer);
        void DeleteOfficer(LoanOfficer officer);
        Task<bool> SaveAsync();
    }
}
