using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using LoanManagement.Services.Interface;

namespace LoanManagement.Services.Implementation
{
    public class LoanSchemeService : ILoanSchemeService
    {
        private readonly ILoanSchemeRepository _schemeRepo;

        public LoanSchemeService(ILoanSchemeRepository schemeRepo)
        {
            _schemeRepo = schemeRepo;
        }

        public async Task<LoanScheme?> GetSchemeByIdAsync(int schemeId)
        {
            return await _schemeRepo.GetByIdAsync(schemeId);
        }

        public async Task<IEnumerable<LoanScheme>> GetAllActiveSchemesAsync()
        {
            return await _schemeRepo.GetAllSchemesAsync();
        }

        public async Task<IEnumerable<LoanScheme>> GetSchemesForAmountAsync(decimal requestedAmount)
        {
            return await _schemeRepo.GetSchemesByMaxAmountAsync(requestedAmount);
        }

        public async Task<bool> CreateSchemeAsync(LoanScheme scheme)
        {
            await _schemeRepo.AddSchemeAsync(scheme);
            return await _schemeRepo.SaveAsync();
        }

        public async Task<bool> UpdateSchemeAsync(LoanScheme scheme)
        {
            _schemeRepo.UpdateScheme(scheme);
            return await _schemeRepo.SaveAsync();
        }

        public async Task<bool> DeactivateSchemeAsync(int schemeId)
        {
            var scheme = await _schemeRepo.GetByIdAsync(schemeId);
            if (scheme == null) return false;

            _schemeRepo.DeleteScheme(scheme);
            return await _schemeRepo.SaveAsync();
        }

        public async Task<decimal> CalculateEMIAsync(int schemeId, decimal loanAmount, int tenureMonths)
        {
            var scheme = await _schemeRepo.GetByIdAsync(schemeId);
            if (scheme == null) return 0;

            var monthlyRate = (double)(scheme.InterestRate / 100 / 12);
            var emi = (double)loanAmount * monthlyRate * Math.Pow(1 + monthlyRate, tenureMonths)
                     / (Math.Pow(1 + monthlyRate, tenureMonths) - 1);

            return Math.Round((decimal)emi, 2);
        }

        public async Task<decimal> CalculateTotalInterestAsync(int schemeId, decimal loanAmount, int tenureMonths)
        {
            var emi = await CalculateEMIAsync(schemeId, loanAmount, tenureMonths);
            var totalAmount = emi * tenureMonths;
            return totalAmount - loanAmount;
        }

        public async Task<bool> IsAmountValidForSchemeAsync(int schemeId, decimal amount)
        {
            var scheme = await _schemeRepo.GetByIdAsync(schemeId);
            return scheme != null && amount <= scheme.MaxAmount;
        }

        public async Task<IEnumerable<LoanScheme>> GetRecommendedSchemesAsync(decimal amount, int preferredTenure)
        {
            var schemes = await _schemeRepo.GetSchemesByMaxAmountAsync(amount);
            return schemes.Where(s => s.DurationsInMonths >= preferredTenure)
                         .OrderBy(s => s.InterestRate);
        }
    }
}
