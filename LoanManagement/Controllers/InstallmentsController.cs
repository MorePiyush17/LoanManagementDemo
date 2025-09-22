using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LoanManagement.Models;
using LoanManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstallmentsController : ControllerBase
    {
        private readonly IInstallmentService _installmentService;

        public InstallmentsController(IInstallmentService installmentService)
        {
            _installmentService = installmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstallmentDetails(int id)
        {
            var installment = await _installmentService.GetInstallmentDetailsAsync(id);
            if (installment == null)
            {
                return NotFound($"Installment with ID {id} not found.");
            }
            return Ok(installment);
        }

        [HttpGet("loan/{loanId}")]
        public async Task<IActionResult> GetLoanEMISchedule(int loanId)
        {
            var installments = await _installmentService.GetLoanEMIScheduleAsync(loanId);
            return Ok(installments);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerUpcomingEMIs(int customerId)
        {
            var installments = await _installmentService.GetCustomerUpcomingEMIsAsync(customerId);
            return Ok(installments);
        }

        [HttpGet("loan/{loanId}/next-due")]
        public async Task<IActionResult> GetNextDueEMI(int loanId)
        {
            var installment = await _installmentService.GetNextDueEMIAsync(loanId);
            if (installment == null)
            {
                return NotFound($"No pending installments found for loan with ID {loanId}.");
            }
            return Ok(installment);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueEMIs()
        {
            var overdueInstallments = await _installmentService.GetOverdueEMIsAsync();
            return Ok(overdueInstallments);
        }

        [HttpGet("due-in-range")]
        public async Task<IActionResult> GetEMIsDueInRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var installments = await _installmentService.GetEMIsDueInRangeAsync(startDate, endDate);
            return Ok(installments);
        }

        [HttpPut("{id}/mark-overdue")]
        public async Task<IActionResult> MarkInstallmentOverdue(int id)
        {
            var result = await _installmentService.MarkInstallmentOverdueAsync(id);
            if (!result)
            {
                return NotFound($"Installment with ID {id} not found.");
            }
            return NoContent();
        }

        [HttpGet("{id}/overdue-days")]
        public async Task<IActionResult> GetOverdueDays(int id)
        {
            var days = await _installmentService.GetOverdueDaysAsync(id);
            if (days == 0)
            {
                return NotFound($"Installment with ID {id} not found or is not overdue.");
            }
            return Ok(new { InstallmentId = id, OverdueDays = days });
        }

        [HttpPost("{id}/process-payment")]
        public async Task<IActionResult> ProcessEMIPayment(int id, [FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _installmentService.ProcessEMIPaymentAsync(id, request.Amount, request.PaymentMethod, request.TransactionId);
            if (!result)
            {
                return StatusCode(500, "A problem occurred while processing the payment.");
            }

            return Ok("Payment processed successfully.");
        }

        [HttpPost("{id}/partial-payment")]
        public async Task<IActionResult> ApplyPartialPayment(int id, [FromBody] PartialPaymentRequest request)
        {
            var result = await _installmentService.ApplyPartialPaymentAsync(id, request.Amount);
            if (!result)
            {
                return StatusCode(500, "A problem occurred while applying the partial payment.");
            }

            return Ok("Partial payment applied successfully.");
        }

        [HttpGet("{id}/penalty")]
        public async Task<IActionResult> CalculatePenalty(int id)
        {
            var penalty = await _installmentService.CalculatePenaltyAsync(id);
            if (penalty == 0)
            {
                return Ok(new { InstallmentId = id, PenaltyAmount = 0, Message = "No penalty is applicable at this time." });
            }
            return Ok(new { InstallmentId = id, PenaltyAmount = penalty });
        }
    }

    public class PaymentRequest
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string PaymentMethod { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
    }

    public class PartialPaymentRequest
    {
        [Required]
        public decimal Amount { get; set; }
    }
}