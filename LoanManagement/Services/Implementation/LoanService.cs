using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using LoanManagement.Services.Interface;

namespace LoanManagement.Services.Implementation
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepo;
        private readonly ILoanApplicationRepository _applicationRepo;
        private readonly IInstallmentRepository _installmentRepo;
        private readonly ILoanSchemeRepository _schemeRepo;

        public LoanService(ILoanRepository loanRepo, ILoanApplicationRepository applicationRepo,
                          IInstallmentRepository installmentRepo, ILoanSchemeRepository schemeRepo)
        {
            _loanRepo = loanRepo;
            _applicationRepo = applicationRepo;
            _installmentRepo = installmentRepo;
            _schemeRepo = schemeRepo;
        }

        public async Task<int> CreateLoanFromApplicationAsync(int applicationId)
        {
            var application = await _applicationRepo.GetApplicationWithDetailsAsync(applicationId);
            if (application == null || application.Status != "Approved")
                return 0;

            var loan = new Loan
            {
                ApplicationId = applicationId,
                CustomerId = application.CustomerId,
                LoanAmount = (decimal)application.LoanAmount,
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now.AddMonths(application.RequestTenureInMonths),
                IsNPA = false
            };

            await _loanRepo.AddLoanAsync(loan);
            if (await _loanRepo.SaveAsync())
            {
                await GenerateInstallmentsAsync(loan.LoanId);
                return loan.LoanId;
            }

            return 0;
        }

        public async Task<Loan?> GetLoanDetailsAsync(int loanId)
        {
            return await _loanRepo.GetByIdAsync(loanId);
        }

        public async Task<IEnumerable<Loan>> GetAllActiveLoansAsync()
        {
            return await _loanRepo.GetAllLoansAsync();
        }

        public async Task<bool> UpdateLoanStatusAsync(int loanId, bool isNPA)
        {
            var loan = await _loanRepo.GetByIdAsync(loanId);
            if (loan == null) return false;

            loan.IsNPA = isNPA;
            _loanRepo.UpdateLoan(loan);
            return await _loanRepo.SaveAsync();
        }

        public async Task<bool> GenerateInstallmentsAsync(int loanId)
        {
            var loan = await _loanRepo.GetLoanWithInstallmentsAsync(loanId);
            if (loan == null || loan.LoanApplication == null) return false;

            var scheme = await _schemeRepo.GetByIdAsync(loan.LoanApplication.SchemeId);
            if (scheme == null) return false;

            var monthlyRate = (double)(scheme.InterestRate / 100 / 12);
            var tenure = loan.LoanApplication.RequestTenureInMonths;
            var emiAmount = (double)loan.LoanAmount * monthlyRate * Math.Pow(1 + monthlyRate, tenure)
                           / (Math.Pow(1 + monthlyRate, tenure) - 1);

            for (int i = 1; i <= tenure; i++)
            {
                var installment = new Installment
                {
                    LoanId = loanId,
                    CustomerId = loan.CustomerId,
                    InstallmentNumber = i,
                    DueDate = loan.IssueDate.AddMonths(i),
                    AmountDue = Math.Round((decimal)emiAmount, 2),
                    AmountPaid = 0,
                    Status = "Pending"
                };

                await _installmentRepo.AddInstallmentAsync(installment);
            }

            return await _installmentRepo.SaveAsync();
        }

        public async Task<Loan?> GetLoanWithEMIScheduleAsync(int loanId)
        {
            return await _loanRepo.GetLoanWithInstallmentsAsync(loanId);
        }

        public async Task<decimal> GetLoanOutstandingAmountAsync(int loanId)
        {
            var installments = await _installmentRepo.GetInstallmentsByLoanAsync(loanId);
            return installments.Where(i => i.Status == "Pending").Sum(i => i.AmountDue - i.AmountPaid);
        }

        public async Task<int> GetRemainingEMIsAsync(int loanId)
        {
            var installments = await _installmentRepo.GetInstallmentsByLoanAsync(loanId);
            return installments.Count(i => i.Status == "Pending");
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
        {
            return await _loanRepo.GetOverdueLoansAsync();
        }

        public async Task<IEnumerable<Loan>> GetNPALoansAsync()
        {
            return await _loanRepo.GetNPALoansAsync();
        }

        public async Task<bool> MarkLoanAsNPAAsync(int loanId, string reason)
        {
            var loan = await _loanRepo.GetByIdAsync(loanId);
            if (loan == null) return false;

            loan.IsNPA = true;
            _loanRepo.UpdateLoan(loan);
            return await _loanRepo.SaveAsync();
        }

        public async Task<decimal> GetTotalPortfolioValueAsync()
        {
            var loans = await _loanRepo.GetAllLoansAsync();
            return loans.Sum(l => l.LoanAmount);
        }
    }

}
