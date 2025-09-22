using LoanManagement.Models;
using LoanManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepaymentsController : ControllerBase
    {
        private readonly IRepaymentService _repaymentService;

        public RepaymentsController(IRepaymentService repaymentService)
        {
            _repaymentService = repaymentService;
        }

        [HttpPost]
        public async Task<IActionResult> RecordPayment([FromBody] Repayment repayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var repaymentId = await _repaymentService.RecordPaymentAsync(repayment);
            if (repaymentId == 0)
            {
                return StatusCode(500, "A problem occurred while recording the payment.");
            }

            return CreatedAtAction(nameof(GetPaymentDetails), new { id = repaymentId }, repayment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentDetails(int id)
        {
            var repayment = await _repaymentService.GetPaymentDetailsAsync(id);
            if (repayment == null)
            {
                return NotFound($"Repayment with ID {id} not found.");
            }
            return Ok(repayment);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerPaymentHistory(int customerId)
        {
            var payments = await _repaymentService.GetCustomerPaymentHistoryAsync(customerId);
            return Ok(payments);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ReversePayment(int id, [FromQuery] string reason)
        {
            var result = await _repaymentService.ReversePaymentAsync(id, reason);
            if (!result)
            {
                return NotFound($"Repayment with ID {id} not found or could not be reversed.");
            }
            return NoContent();
        }

        [HttpGet("daily-collection")]
        public async Task<IActionResult> GetDailyCollection([FromQuery] DateTime date)
        {
            var payments = await _repaymentService.GetDailyCollectionAsync(date);
            return Ok(payments);
        }

        [HttpGet("total-collection")]
        public async Task<IActionResult> GetTotalCollectionInRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var total = await _repaymentService.GetTotalCollectionInRangeAsync(startDate, endDate);
            return Ok(new { StartDate = startDate, EndDate = endDate, TotalCollection = total });
        }

        [HttpGet("method-wise-collection")]
        public async Task<IActionResult> GetPaymentMethodWiseCollection([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var collection = await _repaymentService.GetPaymentMethodWiseCollectionAsync(startDate, endDate);
            return Ok(collection);
        }

        [HttpGet("customer/{customerId}/total-paid")]
        public async Task<IActionResult> GetCustomerTotalPaidAmount(int customerId)
        {
            var totalAmount = await _repaymentService.GetCustomerTotalPaidAmountAsync(customerId);
            return Ok(new { CustomerId = customerId, TotalPaidAmount = totalAmount });
        }

        [HttpPut("{id}/reconcile")]
        public async Task<IActionResult> ReconcilePayment(int id)
        {
            var result = await _repaymentService.ReconcilePaymentAsync(id);
            if (!result)
            {
                return NotFound($"Repayment with ID {id} not found or could not be reconciled.");
            }
            return NoContent();
        }

        [HttpGet("unreconciled")]
        public async Task<IActionResult> GetUnreconciledPayments()
        {
            var payments = await _repaymentService.GetUnreconciledPaymentsAsync();
            // In a real scenario, this would likely be a more specific query
            // to get unreconciled payments. For now, it returns all.
            return Ok(payments);
        }
    }
}