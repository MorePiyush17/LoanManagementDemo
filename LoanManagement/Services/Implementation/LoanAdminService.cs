using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using LoanManagement.Services.Interface;

namespace LoanManagement.Services.Implementation
{
    public class LoanAdminService : ILoanAdminService
    {
        private readonly ILoanAdminRepository _adminRepo;
        private readonly ILoanApplicationRepository _applicationRepo;
        private readonly ILoanRepository _loanRepo;
        private readonly IRepaymentRepository _repaymentRepo;
        private readonly IReportRepository _reportRepo;

        public LoanAdminService(ILoanAdminRepository adminRepo, ILoanApplicationRepository applicationRepo,
                               ILoanRepository loanRepo, IRepaymentRepository repaymentRepo, IReportRepository reportRepo)
        {
            _adminRepo = adminRepo;
            _applicationRepo = applicationRepo;
            _loanRepo = loanRepo;
            _repaymentRepo = repaymentRepo;
            _reportRepo = reportRepo;
        }

        public async Task<LoanAdmin?> GetAdminByIdAsync(int adminId)
        {
            return await _adminRepo.GetByIdAsync(adminId);
        }

        public async Task<LoanAdmin?> GetAdminByUserIdAsync(int userId)
        {
            return await _adminRepo.GetByUserIdAsync(userId);
        }

        public async Task<bool> CreateAdminAsync(LoanAdmin admin)
        {
            await _adminRepo.AddAdminAsync(admin);
            return await _adminRepo.SaveAsync();
        }

        public async Task<bool> UpdateAdminAsync(LoanAdmin admin)
        {
            _adminRepo.UpdateAdmin(admin);
            return await _adminRepo.SaveAsync();
        }

        public async Task<Dictionary<string, int>> GetSystemStatisticsAsync()
        {
            var stats = new Dictionary<string, int>();

            var allApplications = await _applicationRepo.GetAllApplicationsAsync();
            stats["TotalApplications"] = allApplications.Count();
            stats["PendingApplications"] = allApplications.Count(a => a.Status == "Pending");
            stats["ApprovedApplications"] = allApplications.Count(a => a.Status == "Approved");
            stats["RejectedApplications"] = allApplications.Count(a => a.Status == "Rejected");

            var allLoans = await _loanRepo.GetAllLoansAsync();
            stats["TotalActiveLoans"] = allLoans.Count();
            stats["NPALoans"] = allLoans.Count(l => l.IsNPA);

            return stats;
        }

        public async Task<IEnumerable<LoanApplication>> GetPendingApplicationsForReviewAsync()
        {
            return await _applicationRepo.GetPendingApplicationsAsync();
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoansReportAsync()
        {
            return await _loanRepo.GetOverdueLoansAsync();
        }

        public async Task<IEnumerable<Loan>> GetNPALoansReportAsync()
        {
            return await _loanRepo.GetNPALoansAsync();
        }

        public async Task<Report> GenerateCollectionReportAsync(int adminId, DateTime startDate, DateTime endDate)
        {
            var collections = await _repaymentRepo.GetRepaymentsInDateRangeAsync(startDate, endDate);

            var report = new Report
            {
                AdminId = adminId,
                ReportType = "Collection Report",
                StartDate = startDate,
                EndDate = endDate,
                GeneratedDate = DateTime.Now
            };

            await _reportRepo.AddReportAsync(report);
            await _reportRepo.SaveAsync();

            return report;
        }

        public async Task<Report> GenerateLoanPerformanceReportAsync(int adminId, DateTime startDate, DateTime endDate)
        {
            var report = new Report
            {
                AdminId = adminId,
                ReportType = "Loan Performance Report",
                StartDate = startDate,
                EndDate = endDate,
                GeneratedDate = DateTime.Now
            };

            await _reportRepo.AddReportAsync(report);
            await _reportRepo.SaveAsync();

            return report;
        }

        public async Task<IEnumerable<Report>> GetAdminReportsHistoryAsync(int adminId)
        {
            return await _reportRepo.GetReportsByAdminAsync(adminId);
        }
    }
}
