using LoanManagement.Models;

namespace LoanManagement.Repositories.Interface
{
    public interface ILoanAdminRepository
    {
        Task<LoanAdmin?> GetByIdAsync(int adminId);
        Task<LoanAdmin?> GetByUserIdAsync(int userId);
        Task<IEnumerable<LoanAdmin>> GetAllAdminsAsync();
        Task<LoanAdmin?> GetAdminWithReportsAsync(int adminId);
        Task AddAdminAsync(LoanAdmin admin);
        void UpdateAdmin(LoanAdmin admin);
        void DeleteAdmin(LoanAdmin admin);
        Task<bool> SaveAsync();
    }
}
