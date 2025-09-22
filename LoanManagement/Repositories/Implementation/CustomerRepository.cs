using System.Collections.Generic;
using LoanManagement.Data;
using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
namespace LoanManagement.Repositories.Implementation
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        public async Task<Customer?> GetByUserIdAsync(int userId)
        {
            return await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Customer?> GetByContactNumberAsync(string contactNumber)
        {
            return await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.ContactNumber == contactNumber);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomerAsync()
        {
            return await _context.Customers.Include(c => c.User).ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersByCityAsync(string city)
        {
            return await _context.Customers
                .Include(c => c.User)
                .Where(c => c.City == city)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerWithLoansAsync(int customerId)
        {
            return await _context.Customers
                .Include(c => c.Loans)
                    .ThenInclude(l => l.Installments)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<Customer?> GetCustomerWithApplicationsAsync(int customerId)
        {
            return await _context.Customers
                .Include(c => c.LoanApplications)
                    .ThenInclude(la => la.LoanScheme)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

     
    }
}