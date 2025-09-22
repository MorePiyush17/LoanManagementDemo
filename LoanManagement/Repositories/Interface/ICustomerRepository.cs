using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using LoanManagement.Models;
using LoanManagement.Data;

namespace LoanManagement.Repositories.Interface
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int customerid); 
        Task<Customer?> GetByUserIdAsync(int userId);
        Task<Customer?> GetByContactNumberAsync(string contactNumber);
        Task<IEnumerable<Customer>> GetAllCustomerAsync();
        Task<IEnumerable<Customer>> GetCustomersByCityAsync(string city);
        Task<Customer?> GetCustomerWithLoansAsync(int customerId);
        Task<Customer?> GetCustomerWithApplicationsAsync(int customerId);
        Task AddCustomerAsync(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        Task<bool> SaveAsync();

    }
}
