using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using LoanManagement.Services.Interface;

namespace LoanManagement.Services.Implementation
{
    public class InstallmentService : IInstallmentService
    {
        private readonly IInstallmentRepository _installmentRepo;
        private readonly IRepaymentRepository _repaymentRepo;

        public InstallmentService(IInstallmentRepository installmentRepo, IRepaymentRepository repaymentRepo)
        {
            _installmentRepo = installmentRepo;
            _repaymentRepo = repaymentRepo;
        }

        public async Task<Installment?> GetInstallmentDetailsAsync(int installmentId)
        {
            return await _installmentRepo.GetByIdAsync(installmentId);
        }

        public async Task<IEnumerable<Installment>> GetLoanEMIScheduleAsync(int loanId)
        {
            return await _installmentRepo.GetInstallmentsByLoanAsync(loanId);
        }

        public async Task<IEnumerable<Installment>> GetCustomerUpcomingEMIsAsync(int customerId)
        {
            return await _installmentRepo.GetInstallmentsByCustomerAsync(customerId);
        }

        public async Task<Installment?> GetNextDueEMIAsync(int loanId)
        {
            return await _installmentRepo.GetNextDueInstallmentAsync(loanId);
        }

        public async Task<IEnumerable<Installment>> GetOverdueEMIsAsync()
        {
            return await _installmentRepo.GetOverdueInstallmentsAsync();
        }

        public async Task<IEnumerable<Installment>> GetEMIsDueInRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _installmentRepo.GetInstallmentsDueInRangeAsync(startDate, endDate);
        }

        public async Task<bool> MarkInstallmentOverdueAsync(int installmentId)
        {
            var installment = await _installmentRepo.GetByIdAsync(installmentId);
            if (installment == null) return false;

            installment.Status = "Overdue";
            _installmentRepo.UpdateInstallment(installment);
            return await _installmentRepo.SaveAsync();
        }

        public async Task<int> GetOverdueDaysAsync(int installmentId)
        {
            var installment = await _installmentRepo.GetByIdAsync(installmentId);
            if (installment == null) return 0;

            return Math.Max(0, (DateTime.Today - installment.DueDate).Days);
        }

        public async Task<bool> ProcessEMIPaymentAsync(int installmentId, decimal amount, string paymentMethod, string transactionId)
        {
            var installment = await _installmentRepo.GetByIdAsync(installmentId);
            if (installment == null) return false;

            var repayment = new Repayment
            {
                InstallmentId = installmentId,
                CustomerId = installment.CustomerId,
                Amount = amount,
                RepaymentDate = DateTime.Now,
                Method = paymentMethod,
                TransactionId = transactionId
            };

            await _repaymentRepo.AddRepaymentAsync(repayment);

            installment.AmountPaid += amount;
            if (installment.AmountPaid >= installment.AmountDue)
            {
                installment.Status = "Paid";
                installment.PaymentDate = DateTime.Now;
            }

            _installmentRepo.UpdateInstallment(installment);

            return await _repaymentRepo.SaveAsync() && await _installmentRepo.SaveAsync();
        }

        public async Task<bool> ApplyPartialPaymentAsync(int installmentId, decimal amount)
        {
            var installment = await _installmentRepo.GetByIdAsync(installmentId);
            if (installment == null) return false;

            installment.AmountPaid += amount;
            if (installment.AmountPaid < installment.AmountDue)
            {
                installment.Status = "Partial";
            }

            _installmentRepo.UpdateInstallment(installment);
            return await _installmentRepo.SaveAsync();
        }

        public async Task<decimal> CalculatePenaltyAsync(int installmentId)
        {
            var overdueDays = await GetOverdueDaysAsync(installmentId);
            if (overdueDays <= 0) return 0;

            var installment = await _installmentRepo.GetByIdAsync(installmentId);
            if (installment == null) return 0;

            var penaltyRate = 0.02m; // 2% penalty per month
            var monthsOverdue = (decimal)overdueDays / 30;

            return Math.Round(installment.AmountDue * penaltyRate * monthsOverdue, 2);
        }
    }
}
