using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    public interface ILoanSchemeService
    {
        Task<LoanScheme?> GetSchemeByIdAsync(int schemeId);
        Task<IEnumerable<LoanScheme>> GetAllActiveSchemesAsync();
        Task<IEnumerable<LoanScheme>> GetSchemesForAmountAsync(decimal requestedAmount);
        Task<bool> CreateSchemeAsync(LoanScheme scheme);
        Task<bool> UpdateSchemeAsync(LoanScheme scheme);
        Task<bool> DeactivateSchemeAsync(int schemeId);
        Task<decimal> CalculateEMIAsync(int schemeId, decimal loanAmount, int tenureMonths);
        Task<decimal> CalculateTotalInterestAsync(int schemeId, decimal loanAmount, int tenureMonths);
        Task<bool> IsAmountValidForSchemeAsync(int schemeId, decimal amount);
        Task<IEnumerable<LoanScheme>> GetRecommendedSchemesAsync(decimal amount, int preferredTenure);
    }
}