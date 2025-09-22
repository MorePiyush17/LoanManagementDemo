using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    public interface ICustomerService
    {
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task<Customer?> GetCustomerByUserIdAsync(int userId);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<IEnumerable<Customer>> GetCustomersByCityAsync(string city);
        Task<bool> CreateCustomerProfileAsync(Customer customer);
        Task<bool> UpdateCustomerProfileAsync(Customer customer);
        Task<Customer?> GetCustomerPortfolioAsync(int customerId);
        Task<decimal> GetCustomerTotalExposureAsync(int customerId);
        Task<bool> IsCustomerEligibleForLoanAsync(int customerId, decimal requestAmount);
        Task<IEnumerable<Loan>> GetCustomerActiveLoansAsync(int customerId);
        Task<IEnumerable<LoanApplication>> GetCustomerApplicationHistoryAsync(int customerId);
    }
}