using LoanManagement.Models;
using LoanManagement.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // ✅ GET: api/customer
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        // ✅ GET: api/customer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound(new { Message = "Customer not found" });
            return Ok(customer);
        }

        // ✅ GET: api/customer/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCustomerByUserId(int userId)
        {
            var customer = await _customerService.GetCustomerByUserIdAsync(userId);
            if (customer == null) return NotFound(new { Message = "Customer not found" });
            return Ok(customer);
        }

        // ✅ GET: api/customer/city/{city}
        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetCustomersByCity(string city)
        {
            var customers = await _customerService.GetCustomersByCityAsync(city);
            return Ok(customers);
        }

        // ✅ POST: api/customer
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _customerService.CreateCustomerProfileAsync(customer);
            if (!success) return BadRequest(new { Message = "Customer already exists or creation failed" });

            return Ok(new { Message = "Customer created successfully" });
        }

        // ✅ PUT: api/customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (id != customer.CustomerId) return BadRequest();

            var success = await _customerService.UpdateCustomerProfileAsync(customer);
            if (!success) return NotFound(new { Message = "Customer not found or update failed" });

            return Ok(new { Message = "Customer updated successfully" });
        }

        // ✅ GET: api/customer/{id}/portfolio
        [HttpGet("{id}/portfolio")]
        public async Task<IActionResult> GetCustomerPortfolio(int id)
        {
            var customer = await _customerService.GetCustomerPortfolioAsync(id);
            if (customer == null) return NotFound(new { Message = "Customer not found" });
            return Ok(customer);
        }

        // ✅ GET: api/customer/{id}/exposure
        [HttpGet("{id}/exposure")]
        public async Task<IActionResult> GetCustomerExposure(int id)
        {
            var exposure = await _customerService.GetCustomerTotalExposureAsync(id);
            return Ok(new { CustomerId = id, TotalExposure = exposure });
        }

        // ✅ GET: api/customer/{id}/eligibility/{amount}
        [HttpGet("{id}/eligibility/{amount}")]
        public async Task<IActionResult> CheckEligibility(int id, decimal amount)
        {
            var eligible = await _customerService.IsCustomerEligibleForLoanAsync(id, amount);
            return Ok(new { CustomerId = id, RequestedAmount = amount, Eligible = eligible });
        }

        // ✅ GET: api/customer/{id}/loans
        [HttpGet("{id}/loans")]
        public async Task<IActionResult> GetActiveLoans(int id)
        {
            var loans = await _customerService.GetCustomerActiveLoansAsync(id);
            return Ok(loans);
        }

        // ✅ GET: api/customer/{id}/applications
        [HttpGet("{id}/applications")]
        public async Task<IActionResult> GetApplicationHistory(int id)
        {
            var applications = await _customerService.GetCustomerApplicationHistoryAsync(id);
            return Ok(applications);
        }
    }
}
