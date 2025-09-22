using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using LoanManagement.Services.Interface;

namespace LoanManagement.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepo;
        private readonly ILoanRepository _loanRepo;
        private readonly IRepaymentRepository _repaymentRepo;
        private readonly ILoanApplicationRepository _applicationRepo;

        public ReportService(IReportRepository reportRepo, ILoanRepository loanRepo,
                           IRepaymentRepository repaymentRepo, ILoanApplicationRepository applicationRepo)
        {
            _reportRepo = reportRepo;
            _loanRepo = loanRepo;
            _repaymentRepo = repaymentRepo;
            _applicationRepo = applicationRepo;
        }

        public async Task<Report> GenerateLoanPortfolioReportAsync(int adminId, DateTime startDate, DateTime endDate)
        {
            var report = new Report
            {
                AdminId = adminId,
                ReportType = "Loan Portfolio Report",
                StartDate = startDate,
                EndDate = endDate,
                GeneratedDate = DateTime.Now
            };

            await _reportRepo.AddReportAsync(report);
            await _reportRepo.SaveAsync();
            return report;
        }

        public async Task<Report> GenerateCollectionReportAsync(int adminId, DateTime startDate, DateTime endDate)
        {
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

        public async Task<Report> GenerateNPAReportAsync(int adminId)
        {
            var report = new Report
            {
                AdminId = adminId,
                ReportType = "NPA Report",
                StartDate = DateTime.Now.AddMonths(-12),
                EndDate = DateTime.Now,
                GeneratedDate = DateTime.Now
            };

            await _reportRepo.AddReportAsync(report);
            await _reportRepo.SaveAsync();
            return report;
        }

        public async Task<Report> GenerateCustomerReportAsync(int adminId, DateTime startDate, DateTime endDate)
        {
            var report = new Report
            {
                AdminId = adminId,
                ReportType = "Customer Report",
                StartDate = startDate,
                EndDate = endDate,
                GeneratedDate = DateTime.Now
            };

            await _reportRepo.AddReportAsync(report);
            await _reportRepo.SaveAsync();
            return report;
        }

        public async Task<Dictionary<string, object>> GetDashboardMetricsAsync()
        {
            var metrics = new Dictionary<string, object>();

            var loans = await _loanRepo.GetAllLoansAsync();
            metrics["TotalLoans"] = loans.Count();
            metrics["TotalLoanAmount"] = loans.Sum(l => l.LoanAmount);
            metrics["NPACount"] = loans.Count(l => l.IsNPA);

            var applications = await _applicationRepo.GetAllApplicationsAsync();
            metrics["PendingApplications"] = applications.Count(a => a.Status == "Pending");

            return metrics;
        }

        public async Task<IEnumerable<object>> GetLoanTrendAnalysisAsync(int months)
        {
            var startDate = DateTime.Now.AddMonths(-months);
            var applications = await _applicationRepo.GetApplicationsInDateRangeAsync(startDate, DateTime.Now);

            return applications.GroupBy(a => new { a.ApplicationDate.Year, a.ApplicationDate.Month })
                             .Select(g => new {
                                 Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                                 Count = g.Count(),
                                 Amount = g.Sum(a => a.LoanAmount)
                             });
        }

        public async Task<IEnumerable<object>> GetCollectionTrendAnalysisAsync(int months)
        {
            var startDate = DateTime.Now.AddMonths(-months);
            var repayments = await _repaymentRepo.GetRepaymentsInDateRangeAsync(startDate, DateTime.Now);

            return repayments.GroupBy(r => new { r.RepaymentDate.Year, r.RepaymentDate.Month })
                           .Select(g => new {
                               Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                               Count = g.Count(),
                               Amount = g.Sum(r => r.Amount)
                           });
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _reportRepo.GetAllReportsAsync();
        }

        public async Task<Report?> GetReportAsync(int reportId)
        {
            return await _reportRepo.GetByIdAsync(reportId);
        }

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            var report = await _reportRepo.GetByIdAsync(reportId);
            if (report == null) return false;

            _reportRepo.DeleteReport(report);
            return await _reportRepo.SaveAsync();
        }
    }
}
