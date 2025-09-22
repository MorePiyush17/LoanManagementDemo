using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    /// Report Service Interface - Analytics and business intelligence
    /// Report generation, dashboard metrics, business insights
    public interface IReportService
    {
        Task<Report> GenerateLoanPortfolioReportAsync(int adminId, DateTime startDate, DateTime endDate);
        Task<Report> GenerateCollectionReportAsync(int adminId, DateTime startDate, DateTime endDate);
        Task<Report> GenerateNPAReportAsync(int adminId);
        Task<Report> GenerateCustomerReportAsync(int adminId, DateTime startDate, DateTime endDate);
        Task<Dictionary<string, object>> GetDashboardMetricsAsync();
        Task<IEnumerable<object>> GetLoanTrendAnalysisAsync(int months);
        Task<IEnumerable<object>> GetCollectionTrendAnalysisAsync(int months);
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report?> GetReportAsync(int reportId);
        Task<bool> DeleteReportAsync(int reportId);
    }
}
