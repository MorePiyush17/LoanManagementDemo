using LoanManagement.Data;
using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;


namespace LoanManagement.Repositories.Implementation
{
    public class LoanSchemeRepository : ILoanSchemeRepository
    {
        private readonly AppDbContext _context;

        public LoanSchemeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<LoanScheme?> GetByIdAsync(int schemeId)
        {
            return await _context.LoanSchemes.FindAsync(schemeId);
        }

        public async Task<IEnumerable<LoanScheme>> GetAllSchemesAsync()
        {
            return await _context.LoanSchemes.ToListAsync();
        }

        public async Task<IEnumerable<LoanScheme>> GetSchemesByMaxAmountAsync(decimal maxAmount)
        {
            return await _context.LoanSchemes
                .Where(ls => ls.MaxAmount >= maxAmount)
                .ToListAsync();
        }

        public async Task<LoanScheme?> GetSchemeWithApplicationsAsync(int schemeId)
        {
            return await _context.LoanSchemes
                .Include(ls => ls.LoanApplications)
                    .ThenInclude(la => la.Customer)
                        .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(ls => ls.SchemeId == schemeId);
        }

        public async Task AddSchemeAsync(LoanScheme scheme)
        {
            await _context.LoanSchemes.AddAsync(scheme);
        }

        public void UpdateScheme(LoanScheme scheme)
        {
            _context.LoanSchemes.Update(scheme);
        }

        public void DeleteScheme(LoanScheme scheme)
        {
            _context.LoanSchemes.Remove(scheme);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
