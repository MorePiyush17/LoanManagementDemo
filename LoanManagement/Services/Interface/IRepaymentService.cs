using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    /// Repayment Service Interface - Payment processing system
    /// Payment recording, reconciliation, financial tracking
    /// </summary>
    public interface IRepaymentService
    {
        Task<int> RecordPaymentAsync(Repayment repayment);
        Task<Repayment?> GetPaymentDetailsAsync(int repaymentId);
        Task<IEnumerable<Repayment>> GetCustomerPaymentHistoryAsync(int customerId);
        Task<bool> ReversePaymentAsync(int repaymentId, string reason);
        Task<IEnumerable<Repayment>> GetDailyCollectionAsync(DateTime date);
        Task<decimal> GetTotalCollectionInRangeAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<string, decimal>> GetPaymentMethodWiseCollectionAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetCustomerTotalPaidAmountAsync(int customerId);
        Task<bool> ReconcilePaymentAsync(int repaymentId);
        Task<IEnumerable<Repayment>> GetUnreconciledPaymentsAsync();
    }
}