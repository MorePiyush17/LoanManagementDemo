
using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using LoanManagement.Services.Interface;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepo;
    private readonly ILoanRepository _loanRepo;
    private readonly ILoanApplicationRepository _applicationRepo;

    public CustomerService(ICustomerRepository customerRepo, ILoanRepository loanRepo,
                          ILoanApplicationRepository applicationRepo)
    {
        _customerRepo = customerRepo;
        _loanRepo = loanRepo;
        _applicationRepo = applicationRepo;
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        return await _customerRepo.GetByIdAsync(customerId);
    }

    public async Task<Customer?> GetCustomerByUserIdAsync(int userId)
    {
        return await _customerRepo.GetByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepo.GetAllCustomerAsync();
    }

    public async Task<IEnumerable<Customer>> GetCustomersByCityAsync(string city)
    {
        return await _customerRepo.GetCustomersByCityAsync(city);
    }

    public async Task<bool> CreateCustomerProfileAsync(Customer customer)
    {
        var existingCustomer = await _customerRepo.GetByContactNumberAsync(customer.ContactNumber);
        if (existingCustomer != null) return false;

        await _customerRepo.AddCustomerAsync(customer);
        return await _customerRepo.SaveAsync();
    }

    public async Task<bool> UpdateCustomerProfileAsync(Customer customer)
    {
        _customerRepo.UpdateCustomer(customer);
        return await _customerRepo.SaveAsync();
    }

    public async Task<Customer?> GetCustomerPortfolioAsync(int customerId)
    {
        return await _customerRepo.GetCustomerWithLoansAsync(customerId);
    }

    public async Task<decimal> GetCustomerTotalExposureAsync(int customerId)
    {
        return await _loanRepo.GetTotalLoanAmountByCustomerAsync(customerId);
    }

    public async Task<bool> IsCustomerEligibleForLoanAsync(int customerId, decimal requestAmount)
    {
        var currentExposure = await GetCustomerTotalExposureAsync(customerId);
        var maxExposureLimit = 1000000m;

        if (currentExposure + requestAmount > maxExposureLimit)
            return false;

        var customer = await _customerRepo.GetCustomerWithLoansAsync(customerId);
        if (customer?.Loans?.Any(l => l.Installments.Any(i => i.Status == "Overdue")) == true)
            return false;

        return true;
    }

    public async Task<IEnumerable<Loan>> GetCustomerActiveLoansAsync(int customerId)
    {
        return await _loanRepo.GetLoansByCustomerAsync(customerId);
    }

    public async Task<IEnumerable<LoanApplication>> GetCustomerApplicationHistoryAsync(int customerId)
    {
        return await _applicationRepo.GetApplicationsByCustomerAsync(customerId);
    }
}