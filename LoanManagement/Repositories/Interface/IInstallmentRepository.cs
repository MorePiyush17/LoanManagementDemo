using LoanManagement.Models;

namespace LoanManagement.Repositories.Interface
{
    public interface IInstallmentRepository
    {
        Task<Installment?> GetByIdAsync(int installmentId);
        Task<IEnumerable<Installment>> GetAllInstallmentsAsync();
        Task<IEnumerable<Installment>> GetInstallmentsByLoanAsync(int loanId);
        Task<IEnumerable<Installment>> GetInstallmentsByCustomerAsync(int customerId);
        Task<IEnumerable<Installment>> GetPendingInstallmentsAsync();
        Task<IEnumerable<Installment>> GetOverdueInstallmentsAsync();
        Task<IEnumerable<Installment>> GetInstallmentsDueInRangeAsync(DateTime startDate, DateTime endDate);
        Task<Installment?> GetNextDueInstallmentAsync(int loanId);
        Task AddInstallmentAsync(Installment installment);
        void UpdateInstallment(Installment installment);
        void DeleteInstallment(Installment installment);
        Task<bool> SaveAsync();
    }
}
