using LoanManagement.Data;
using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Repositories.Implementation
{
    public class LoanAdminRepository: ILoanAdminRepository
    {
        private readonly AppDbContext _context;

        public LoanAdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoanAdmin?> GetByIdAsync(int adminId)
        {
            return await _context.LoanAdmins.FindAsync(adminId);
        }

        public async Task<LoanAdmin?> GetByUserIdAsync(int userId)
        {
            return await _context.LoanAdmins
                .Include(la => la.User)
                .FirstOrDefaultAsync(la => la.UserId == userId);
        }

        public async Task<IEnumerable<LoanAdmin>> GetAllAdminsAsync()
        {
            return await _context.LoanAdmins.Include(la => la.User).ToListAsync();
        }

        public async Task<LoanAdmin?> GetAdminWithReportsAsync(int adminId)
        {
            return await _context.LoanAdmins
                .Include(la => la.Reports)
                .Include(la => la.User)
                .FirstOrDefaultAsync(la => la.AdminId == adminId);
        }

        public async Task AddAdminAsync(LoanAdmin admin)
        {
            await _context.LoanAdmins.AddAsync(admin);
        }

        public void UpdateAdmin(LoanAdmin admin)
        {
            _context.LoanAdmins.Update(admin);
        }

        public void DeleteAdmin(LoanAdmin admin)
        {
            _context.LoanAdmins.Remove(admin);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
