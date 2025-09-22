using LoanManagement.Data;
using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Repositories.Implementation
{
    public class LoanApplicationRepository :  ILoanApplicationRepository
    {
        private readonly AppDbContext _context;

        public LoanApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoanApplication?> GetByIdAsync(int applicationId)
        {
            return await _context.LoanApplications.FindAsync(applicationId);
        }

        public async Task<IEnumerable<LoanApplication>> GetAllApplicationsAsync()
        {
            return await _context.LoanApplications
                .Include(la => la.Customer)
                    .ThenInclude(c => c.User)
                .Include(la => la.LoanScheme)
                .OrderByDescending(la => la.ApplicationDate)
                .ToListAsync();
        }

        public async Task<LoanApplication?> GetApplicationWithDetailsAsync(int applicationId)
        {
            return await _context.LoanApplications
                .Include(la => la.Customer)
                    .ThenInclude(c => c.User)
                .Include(la => la.LoanOfficer)
                    .ThenInclude(lo => lo.User)
                .Include(la => la.LoanScheme)
                .Include(la => la.Documents)
                .Include(la => la.Loan)
                .FirstOrDefaultAsync(la => la.ApplicationId == applicationId);
        }

        public async Task<IEnumerable<LoanApplication>> GetApplicationsByCustomerAsync(int customerId)
        {
            return await _context.LoanApplications
                .Include(la => la.LoanScheme)
                .Include(la => la.LoanOfficer)
                    .ThenInclude(lo => lo.User)
                .Where(la => la.CustomerId == customerId)
                .OrderByDescending(la => la.ApplicationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LoanApplication>> GetApplicationsByOfficerAsync(int officerId)
        {
            return await _context.LoanApplications
                .Include(la => la.Customer)
                    .ThenInclude(c => c.User)
                .Include(la => la.LoanScheme)
                .Where(la => la.AssignedOfficerId == officerId)
                .OrderByDescending(la => la.ApplicationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LoanApplication>> GetApplicationsByStatusAsync(string status)
        {
            return await _context.LoanApplications
                .Include(la => la.Customer)
                    .ThenInclude(c => c.User)
                .Include(la => la.LoanOfficer)
                    .ThenInclude(lo => lo.User)
                .Include(la => la.LoanScheme)
                .Where(la => la.Status == status)
                .OrderByDescending(la => la.ApplicationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LoanApplication>> GetPendingApplicationsAsync()
        {
            return await GetApplicationsByStatusAsync("Pending");
        }

        public async Task<IEnumerable<LoanApplication>> GetApplicationsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.LoanApplications
                .Include(la => la.Customer)
                    .ThenInclude(c => c.User)
                .Include(la => la.LoanScheme)
                .Where(la => la.ApplicationDate >= startDate && la.ApplicationDate <= endDate)
                .OrderByDescending(la => la.ApplicationDate)
                .ToListAsync();
        }

        public async Task AddApplicationAsync(LoanApplication application)
        {
            await _context.LoanApplications.AddAsync(application);
        }

        public void UpdateApplication(LoanApplication application)
        {
            _context.LoanApplications.Update(application);
        }

        public void DeleteApplication(LoanApplication application)
        {
            _context.LoanApplications.Remove(application);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
