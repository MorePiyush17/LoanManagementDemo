using LoanManagement.Models;

namespace LoanManagement.Repositories.Interface
{
    public interface ILoanRepository
    {
        Task<Loan?> GetByIdAsync(int loanId);
        Task<IEnumerable<Loan>> GetAllLoansAsync();
        Task<Loan?> GetLoanWithInstallmentsAsync(int loanId);
        Task<IEnumerable<Loan>> GetLoansByCustomerAsync(int customerId);
        Task<IEnumerable<Loan>> GetOverdueLoansAsync();
        Task<IEnumerable<Loan>> GetNPALoansAsync();
        Task<Loan?> GetLoanByApplicationIdAsync(int applicationId);
        Task<decimal> GetTotalLoanAmountByCustomerAsync(int customerId);
        Task AddLoanAsync(Loan loan);
        void UpdateLoan(Loan loan);
        void DeleteLoan(Loan loan);
        Task<bool> SaveAsync();
    }
}
