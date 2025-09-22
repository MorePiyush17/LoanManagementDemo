using LoanManagement.Data;
using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;


namespace LoanManagement.Repositories.Implementation
{
    public class LoanOfficerRepository : ILoanOfficerRepository
    {
        private readonly AppDbContext _context;

        public LoanOfficerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoanOfficer?> GetByIdAsync(int officerId)
        {
            return await _context.LoanOfficers.FindAsync(officerId);
        }

        public async Task<LoanOfficer?> GetByUserIdAsync(int userId)
        {
            return await _context.LoanOfficers
                .Include(lo => lo.User)
                .FirstOrDefaultAsync(lo => lo.UserId == userId);
        }

        public async Task<IEnumerable<LoanOfficer>> GetAllOfficersAsync()
        {
            return await _context.LoanOfficers.Include(lo => lo.User).ToListAsync();
        }

        public async Task<IEnumerable<LoanOfficer>> GetOfficersByCityAsync(string city)
        {
            return await _context.LoanOfficers
                .Include(lo => lo.User)
                .Where(lo => lo.City == city)
                .ToListAsync();
        }

        public async Task<LoanOfficer?> GetOfficerWithApplicationsAsync(int officerId)
        {
            return await _context.LoanOfficers
                .Include(lo => lo.AssignedApplications)
                    .ThenInclude(la => la.Customer)
                        .ThenInclude(c => c.User)
                .Include(lo => lo.User)
                .FirstOrDefaultAsync(lo => lo.OfficerId == officerId);
        }

        public async Task<int> GetApplicationCountAsync(int officerId)
        {
            return await _context.LoanApplications
                .CountAsync(la => la.AssignedOfficerId == officerId);
        }

        public async Task AddOfficerAsync(LoanOfficer officer)
        {
            await _context.LoanOfficers.AddAsync(officer);
        }

        public void UpdateOfficer(LoanOfficer officer)
        {
            _context.LoanOfficers.Update(officer);
        }

        public void DeleteOfficer(LoanOfficer officer)
        {
            _context.LoanOfficers.Remove(officer);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
