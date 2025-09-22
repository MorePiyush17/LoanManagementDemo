using LoanManagement.Data;
using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;


namespace LoanManagement.Repositories.Implementation
{
    public class InstallmentRepository: IInstallmentRepository
    {
        private readonly AppDbContext _context;

        public InstallmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Installment?> GetByIdAsync(int installmentId)
        {
            return await _context.Installments.FindAsync(installmentId);
        }

        public async Task<IEnumerable<Installment>> GetAllInstallmentsAsync()
        {
            return await _context.Installments
                .Include(i => i.Customer)
                    .ThenInclude(c => c.User)
                .Include(i => i.Loan)
                .OrderBy(i => i.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Installment>> GetInstallmentsByLoanAsync(int loanId)
        {
            return await _context.Installments
                .Include(i => i.Repayment)
                .Where(i => i.LoanId == loanId)
                .OrderBy(i => i.InstallmentNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<Installment>> GetInstallmentsByCustomerAsync(int customerId)
        {
            return await _context.Installments
                .Include(i => i.Loan)
                    .ThenInclude(l => l.LoanApplication)
                        .ThenInclude(la => la.LoanScheme)
                .Include(i => i.Repayment)
                .Where(i => i.CustomerId == customerId)
                .OrderBy(i => i.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Installment>> GetPendingInstallmentsAsync()
        {
            return await _context.Installments
                .Include(i => i.Customer)
                    .ThenInclude(c => c.User)
                .Include(i => i.Loan)
                .Where(i => i.Status == "Pending")
                .OrderBy(i => i.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Installment>> GetOverdueInstallmentsAsync()
        {
            var today = DateTime.Today;
            return await _context.Installments
                .Include(i => i.Customer)
                    .ThenInclude(c => c.User)
                .Include(i => i.Loan)
                .Where(i => i.DueDate < today && i.Status != "Paid")
                .OrderBy(i => i.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Installment>> GetInstallmentsDueInRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Installments
                .Include(i => i.Customer)
                    .ThenInclude(c => c.User)
                .Include(i => i.Loan)
                .Where(i => i.DueDate >= startDate && i.DueDate <= endDate)
                .OrderBy(i => i.DueDate)
                .ToListAsync();
        }

        public async Task<Installment?> GetNextDueInstallmentAsync(int loanId)
        {
            var today = DateTime.Today;
            return await _context.Installments
                .Where(i => i.LoanId == loanId && i.Status == "Pending")
                .OrderBy(i => i.DueDate)
                .FirstOrDefaultAsync();
        }

        public async Task AddInstallmentAsync(Installment installment)
        {
            await _context.Installments.AddAsync(installment);
        }

        public void UpdateInstallment(Installment installment)
        {
            _context.Installments.Update(installment);
        }

        public void DeleteInstallment(Installment installment)
        {
            _context.Installments.Remove(installment);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
