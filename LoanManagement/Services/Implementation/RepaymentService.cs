using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using LoanManagement.Services.Interface;

namespace LoanManagement.Services.Implementation
{
    public class RepaymentService : IRepaymentService
    {
        private readonly IRepaymentRepository _repaymentRepo;

        public RepaymentService(IRepaymentRepository repaymentRepo)
        {
            _repaymentRepo = repaymentRepo;
        }

        public async Task<int> RecordPaymentAsync(Repayment repayment)
        {
            repayment.RepaymentDate = DateTime.Now;
            await _repaymentRepo.AddRepaymentAsync(repayment);

            if (await _repaymentRepo.SaveAsync())
                return repayment.RepaymentId;

            return 0;
        }

        public async Task<Repayment?> GetPaymentDetailsAsync(int repaymentId)
        {
            return await _repaymentRepo.GetByIdAsync(repaymentId);
        }

        public async Task<IEnumerable<Repayment>> GetCustomerPaymentHistoryAsync(int customerId)
        {
            return await _repaymentRepo.GetRepaymentsByCustomerAsync(customerId);
        }

        public async Task<bool> ReversePaymentAsync(int repaymentId, string reason)
        {
            var repayment = await _repaymentRepo.GetByIdAsync(repaymentId);
            if (repayment == null) return false;

            _repaymentRepo.DeleteRepayment(repayment);
            return await _repaymentRepo.SaveAsync();
        }

        public async Task<IEnumerable<Repayment>> GetDailyCollectionAsync(DateTime date)
        {
            return await _repaymentRepo.GetRepaymentsInDateRangeAsync(date, date);
        }

        public async Task<decimal> GetTotalCollectionInRangeAsync(DateTime startDate, DateTime endDate)
        {
            var repayments = await _repaymentRepo.GetRepaymentsInDateRangeAsync(startDate, endDate);
            return repayments.Sum(r => r.Amount);
        }

        public async Task<Dictionary<string, decimal>> GetPaymentMethodWiseCollectionAsync(DateTime startDate, DateTime endDate)
        {
            var repayments = await _repaymentRepo.GetRepaymentsInDateRangeAsync(startDate, endDate);

            return repayments.GroupBy(r => r.Method)
                           .ToDictionary(g => g.Key, g => g.Sum(r => r.Amount));
        }

        public async Task<decimal> GetCustomerTotalPaidAmountAsync(int customerId)
        {
            return await _repaymentRepo.GetTotalRepaymentsByCustomerAsync(customerId);
        }

        public async Task<bool> ReconcilePaymentAsync(int repaymentId)
        {
            var repayment = await _repaymentRepo.GetByIdAsync(repaymentId);
            if (repayment == null) return false;

            _repaymentRepo.UpdateRepayment(repayment);
            return await _repaymentRepo.SaveAsync();
        }

        public async Task<IEnumerable<Repayment>> GetUnreconciledPaymentsAsync()
        {
            return await _repaymentRepo.GetAllRepaymentsAsync();
        }
    }

}
