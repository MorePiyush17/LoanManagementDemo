using LoanManagement.Models;

namespace LoanManagement.Repositories.Interface
{
    public interface ILoanSchemeRepository
    {
        Task<LoanScheme?> GetByIdAsync(int schemeId);
        Task<IEnumerable<LoanScheme>> GetAllSchemesAsync();
        Task<IEnumerable<LoanScheme>> GetSchemesByMaxAmountAsync(decimal maxAmount);
        Task<LoanScheme?> GetSchemeWithApplicationsAsync(int schemeId);
        Task AddSchemeAsync(LoanScheme scheme);
        void UpdateScheme(LoanScheme scheme);
        void DeleteScheme(LoanScheme scheme);
        Task<bool> SaveAsync();
    }
}
