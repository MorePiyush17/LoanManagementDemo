using LoanManagement.Models;

namespace LoanManagement.Repositories.Interface
{
    public interface IReportRepository
    {
        Task<Report?> GetByIdAsync(int reportId);
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<IEnumerable<Report>> GetReportsByAdminAsync(int adminId);
        Task<IEnumerable<Report>> GetReportsByTypeAsync(string reportType);
        Task<IEnumerable<Report>> GetReportsInDateRangeAsync(DateTime startDate, DateTime endDate);
        Task AddReportAsync(Report report);
        void UpdateReport(Report report);
        void DeleteReport(Report report);
        Task<bool> SaveAsync();
    }
}
