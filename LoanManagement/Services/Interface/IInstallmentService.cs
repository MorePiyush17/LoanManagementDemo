using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    /// Installment Service Interface - EMI management system
    /// Payment scheduling, overdue tracking, collection management
    public interface IInstallmentService
    {
        Task<Installment?> GetInstallmentDetailsAsync(int installmentId);
        Task<IEnumerable<Installment>> GetLoanEMIScheduleAsync(int loanId);
        Task<IEnumerable<Installment>> GetCustomerUpcomingEMIsAsync(int customerId);
        Task<Installment?> GetNextDueEMIAsync(int loanId);
        Task<IEnumerable<Installment>> GetOverdueEMIsAsync();
        Task<IEnumerable<Installment>> GetEMIsDueInRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> MarkInstallmentOverdueAsync(int installmentId);
        Task<int> GetOverdueDaysAsync(int installmentId);
        Task<bool> ProcessEMIPaymentAsync(int installmentId, decimal amount, string paymentMethod, string transactionId);
        Task<bool> ApplyPartialPaymentAsync(int installmentId, decimal amount);
        Task<decimal> CalculatePenaltyAsync(int installmentId);
    }
}