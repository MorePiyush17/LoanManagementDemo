using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    /// LoanAdmin Service Interface - Administrative functions
    /// System administration, reporting, scheme management
    /// </summary>
    public interface ILoanAdminService
    {
        Task<LoanAdmin?> GetAdminByIdAsync(int adminId);
        Task<LoanAdmin?> GetAdminByUserIdAsync(int userId);
        Task<bool> CreateAdminAsync(LoanAdmin admin);
        Task<bool> UpdateAdminAsync(LoanAdmin admin);
        Task<Dictionary<string, int>> GetSystemStatisticsAsync();
        Task<IEnumerable<LoanApplication>> GetPendingApplicationsForReviewAsync();
        Task<IEnumerable<Loan>> GetOverdueLoansReportAsync();
        Task<IEnumerable<Loan>> GetNPALoansReportAsync();
        Task<Report> GenerateCollectionReportAsync(int adminId, DateTime startDate, DateTime endDate);
        Task<Report> GenerateLoanPerformanceReportAsync(int adminId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Report>> GetAdminReportsHistoryAsync(int adminId);
    }
}