using LoanManagement.Models;

namespace LoanManagement.Repositories.Interface
{
    public interface IRepaymentRepository
    {
        Task<Repayment?> GetByIdAsync(int repaymentId);
        Task<IEnumerable<Repayment>> GetAllRepaymentsAsync();
        Task<IEnumerable<Repayment>> GetRepaymentsByCustomerAsync(int customerId);
        Task<IEnumerable<Repayment>> GetRepaymentsInDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Repayment?> GetRepaymentByInstallmentAsync(int installmentId);
        Task<decimal> GetTotalRepaymentsByCustomerAsync(int customerId);
        Task<IEnumerable<Repayment>> GetRepaymentsByMethodAsync(string method);
        Task AddRepaymentAsync(Repayment repayment);
        void UpdateRepayment(Repayment repayment);
        void DeleteRepayment(Repayment repayment);
        Task<bool> SaveAsync();
    }
}
