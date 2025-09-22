using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    public interface ILoanService
    {
        Task<int> CreateLoanFromApplicationAsync(int applicationId);
        Task<Loan?> GetLoanDetailsAsync(int loanId);
        Task<IEnumerable<Loan>> GetAllActiveLoansAsync();
        Task<bool> UpdateLoanStatusAsync(int loanId, bool isNPA);
        Task<bool> GenerateInstallmentsAsync(int loanId);
        Task<Loan?> GetLoanWithEMIScheduleAsync(int loanId);
        Task<decimal> GetLoanOutstandingAmountAsync(int loanId);
        Task<int> GetRemainingEMIsAsync(int loanId);
        Task<IEnumerable<Loan>> GetOverdueLoansAsync();
        Task<IEnumerable<Loan>> GetNPALoansAsync();
        Task<bool> MarkLoanAsNPAAsync(int loanId, string reason);
        Task<decimal> GetTotalPortfolioValueAsync();
    }
}