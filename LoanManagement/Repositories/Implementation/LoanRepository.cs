using LoanManagement.Data;
using LoanManagement.Models;
using Microsoft.EntityFrameworkCore;
using LoanManagement.Repositories.Interface;


namespace LoanManagement.Repositories.Implementation
{
    public class LoanRepository : ILoanRepository
    {

        private readonly AppDbContext _context;

        public LoanRepository(AppDbContext context)
        {
            _context = context;

        }
        public async Task<Loan?> GetByIdAsync(int loanId)
        {
            return await _context.Loans.FindAsync(loanId);

        }
        public async Task<IEnumerable<Loan>> GetAllLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.Customer)
                    .ThenInclude(c => c.User)
                .Include(l => l.LoanApplication)
                    .ThenInclude(la => la.LoanScheme)
                .OrderByDescending(l => l.IssueDate)
                .ToListAsync();
        }
        public async Task<Loan?> GetLoanWithInstallmentsAsync(int loanId)
        {
            return await _context.Loans
                .Include(l => l.Installments.OrderBy(i => i.InstallmentNumber))
                    .ThenInclude(i => i.Repayment)
                .Include(l => l.Customer)
                    .ThenInclude(c => c.User)
                .Include(l => l.LoanApplication)
                    .ThenInclude(la => la.LoanScheme)
                .FirstOrDefaultAsync(l => l.LoanId == loanId);
        }

        public async Task<IEnumerable<Loan>> GetLoansByCustomerAsync(int customerId)
        {
            return await _context.Loans
                .Include(l => l.LoanApplication)
                    .ThenInclude(la => la.LoanScheme)
                .Where(l => l.CustomerId == customerId)
                .OrderByDescending(l => l.IssueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
        {
            var today = DateTime.Today;
            return await _context.Loans
                .Include(l => l.Customer)
                    .ThenInclude(c => c.User)
                .Include(l => l.Installments)
                .Where(l => l.Installments.Any(i => i.DueDate < today && i.Status != "Paid"))
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetNPALoansAsync()
        {
            return await _context.Loans
                .Include(l => l.Customer)
                    .ThenInclude(c => c.User)
                .Include(l => l.LoanApplication)
                    .ThenInclude(la => la.LoanScheme)
                .Where(l => l.IsNPA)
                .ToListAsync();
        }

        public async Task<Loan?> GetLoanByApplicationIdAsync(int applicationId)
        {
            return await _context.Loans
                .Include(l => l.LoanApplication)
                .Include(l => l.Customer)
                .FirstOrDefaultAsync(l => l.ApplicationId == applicationId);
        }

        public async Task<decimal> GetTotalLoanAmountByCustomerAsync(int customerId)
        {
            return await _context.Loans
                .Where(l => l.CustomerId == customerId)
                .SumAsync(l => l.LoanAmount);
        }

        public async Task AddLoanAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
        }

        public void UpdateLoan(Loan loan)
        {
            _context.Loans.Update(loan);
        }

        public void DeleteLoan(Loan loan)
        {
            _context.Loans.Remove(loan);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
