using LoanManagement.Data;
using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;


namespace LoanManagement.Repositories.Implementation
{
    // <summary>
    /// Repayment Repository - Payment processing aur history
    /// Customer payments ka complete record
    /// </summary>
    public class RepaymentRepository : IRepaymentRepository
    {
        private readonly AppDbContext _context;

        public RepaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Repayment?> GetByIdAsync(int repaymentId)
        {
            return await _context.Repayments.FindAsync(repaymentId);
        }

        /// <summary>
        /// All payments - Daily collection report ke liye
        /// </summary>
        public async Task<IEnumerable<Repayment>> GetAllRepaymentsAsync()
        {
            return await _context.Repayments
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .Include(r => r.Installment)
                    .ThenInclude(i => i.Loan)
                .OrderByDescending(r => r.RepaymentDate)  // Latest payments first
                .ToListAsync();
        }

        /// <summary>
        /// Customer payment history - Customer portal aur support ke liye
        /// </summary>
        public async Task<IEnumerable<Repayment>> GetRepaymentsByCustomerAsync(int customerId)
        {
            return await _context.Repayments
                .Include(r => r.Installment)
                    .ThenInclude(i => i.Loan)                    // Kiske loan ka payment
                        .ThenInclude(l => l.LoanApplication)
                            .ThenInclude(la => la.LoanScheme)    // Loan type
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.RepaymentDate)
                .ToListAsync();
        }

        /// <summary>
        /// Daily collection report - Date wise payment summary
        /// Branch wise daily collection ke liye
        /// </summary>
        public async Task<IEnumerable<Repayment>> GetRepaymentsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Repayments
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .Include(r => r.Installment)
                .Where(r => r.RepaymentDate >= startDate && r.RepaymentDate <= endDate)
                .OrderByDescending(r => r.RepaymentDate)
                .ToListAsync();
        }

        /// <summary>
        /// Specific EMI ka payment - EMI paid hai ya nahi check karne ke liye
        /// </summary>
        public async Task<Repayment?> GetRepaymentByInstallmentAsync(int installmentId)
        {
            return await _context.Repayments
                .Include(r => r.Customer)
                .Include(r => r.Installment)
                .FirstOrDefaultAsync(r => r.InstallmentId == installmentId);
        }

        /// <summary>
        /// Customer ka total paid amount - Portfolio analysis ke liye
        /// </summary>
        public async Task<decimal> GetTotalRepaymentsByCustomerAsync(int customerId)
        {
            return await _context.Repayments
                .Where(r => r.CustomerId == customerId)
                .SumAsync(r => r.Amount);
        }

        /// <summary>
        /// Payment method wise analysis - Cash, Online, Cheque collections
        /// Channel performance analysis ke liye
        /// </summary>
        public async Task<IEnumerable<Repayment>> GetRepaymentsByMethodAsync(string method)
        {
            return await _context.Repayments
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .Where(r => r.Method == method)
                .OrderByDescending(r => r.RepaymentDate)
                .ToListAsync();
        }

        public async Task AddRepaymentAsync(Repayment repayment)
        {
            await _context.Repayments.AddAsync(repayment);
        }

        public void UpdateRepayment(Repayment repayment)
        {
            _context.Repayments.Update(repayment);
        }

        public void DeleteRepayment(Repayment repayment)
        {
            _context.Repayments.Remove(repayment);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

