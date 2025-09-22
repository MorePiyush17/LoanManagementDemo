using LoanManagement.Data;
using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Repositories.Implementation
{
    // <summary>
    /// Report Repository - Analytics aur reporting system
    /// Management information system ke liye
    /// </summary>
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Report?> GetByIdAsync(int reportId)
        {
            return await _context.Reports.FindAsync(reportId);
        }

        /// <summary>
        /// All generated reports - Report management dashboard
        /// </summary>
        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _context.Reports
                .Include(r => r.LoanAdmin)
                    .ThenInclude(la => la.User)
                .OrderByDescending(r => r.GeneratedDate)  // Latest reports first
                .ToListAsync();
        }

        /// <summary>
        /// Admin specific reports - Admin ki generated reports history
        /// </summary>
        public async Task<IEnumerable<Report>> GetReportsByAdminAsync(int adminId)
        {
            return await _context.Reports
                .Where(r => r.AdminId == adminId)
                .OrderByDescending(r => r.GeneratedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Report type wise filtering - Monthly, Quarterly, Annual reports
        /// Collection Report, NPA Report, etc categories
        /// </summary>
        public async Task<IEnumerable<Report>> GetReportsByTypeAsync(string reportType)
        {
            return await _context.Reports
                .Include(r => r.LoanAdmin)
                    .ThenInclude(la => la.User)
                .Where(r => r.ReportType == reportType)
                .OrderByDescending(r => r.GeneratedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Time period wise reports - Historical report analysis
        /// </summary>
        public async Task<IEnumerable<Report>> GetReportsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Reports
                .Include(r => r.LoanAdmin)
                    .ThenInclude(la => la.User)
                .Where(r => r.StartDate >= startDate && r.EndDate <= endDate)
                .OrderByDescending(r => r.GeneratedDate)
                .ToListAsync();
        }

        public async Task AddReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
        }

        public void UpdateReport(Report report)
        {
            _context.Reports.Update(report);
        }

        public void DeleteReport(Report report)
        {
            _context.Reports.Remove(report);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
