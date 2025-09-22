using LoanManagement.Models;
using LoanManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost("from-application/{applicationId}")]
        public async Task<IActionResult> CreateLoanFromApplication(int applicationId)
        {
            var loanId = await _loanService.CreateLoanFromApplicationAsync(applicationId);
            if (loanId == 0)
            {
                return BadRequest("Loan creation failed. Application may not be approved or does not exist.");
            }
            return CreatedAtAction(nameof(GetLoanDetails), new { id = loanId }, new { LoanId = loanId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanDetails(int id)
        {
            var loan = await _loanService.GetLoanDetailsAsync(id);
            if (loan == null)
            {
                return NotFound($"Loan with ID {id} not found.");
            }
            return Ok(loan);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActiveLoans()
        {
            var loans = await _loanService.GetAllActiveLoansAsync();
            return Ok(loans);
        }

        [HttpGet("{id}/emi-schedule")]
        public async Task<IActionResult> GetLoanWithEMISchedule(int id)
        {
            var loan = await _loanService.GetLoanWithEMIScheduleAsync(id);
            if (loan == null)
            {
                return NotFound($"Loan with ID {id} not found.");
            }
            return Ok(loan);
        }

        [HttpGet("{id}/outstanding-amount")]
        public async Task<IActionResult> GetLoanOutstandingAmount(int id)
        {
            var outstandingAmount = await _loanService.GetLoanOutstandingAmountAsync(id);
            return Ok(new { LoanId = id, OutstandingAmount = outstandingAmount });
        }

        [HttpGet("{id}/remaining-emis")]
        public async Task<IActionResult> GetRemainingEMIs(int id)
        {
            var remainingEMIs = await _loanService.GetRemainingEMIsAsync(id);
            return Ok(new { LoanId = id, RemainingEMIs = remainingEMIs });
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueLoans()
        {
            var overdueLoans = await _loanService.GetOverdueLoansAsync();
            return Ok(overdueLoans);
        }

        [HttpGet("npa")]
        public async Task<IActionResult> GetNPALoans()
        {
            var npaLoans = await _loanService.GetNPALoansAsync();
            return Ok(npaLoans);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateLoanStatus(int id, [FromQuery] bool isNPA)
        {
            var result = await _loanService.UpdateLoanStatusAsync(id, isNPA);
            if (!result)
            {
                return NotFound($"Loan with ID {id} not found.");
            }
            return NoContent();
        }

        [HttpPut("{id}/mark-npa")]
        public async Task<IActionResult> MarkLoanAsNPA(int id, [FromBody] MarkNPARequest request)
        {
            var result = await _loanService.MarkLoanAsNPAAsync(id, request.Reason);
            if (!result)
            {
                return NotFound($"Loan with ID {id} not found.");
            }
            return NoContent();
        }

        [HttpGet("total-portfolio-value")]
        public async Task<IActionResult> GetTotalPortfolioValue()
        {
            var totalValue = await _loanService.GetTotalPortfolioValueAsync();
            return Ok(new { TotalPortfolioValue = totalValue });
        }
    }

    public class MarkNPARequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}