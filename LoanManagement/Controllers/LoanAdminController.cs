using LoanManagement.Models;
using LoanManagement.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LoanAdminsController : ControllerBase
    {
        private readonly ILoanAdminService _loanAdminService;

        public LoanAdminsController(ILoanAdminService loanAdminService)
        {
            _loanAdminService = loanAdminService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _loanAdminService.GetAdminByIdAsync(id);
            if (admin == null)
            {
                return NotFound($"LoanAdmin with ID {id} not found.");
            }
            return Ok(admin);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAdminByUserId(int userId)
        {
            var admin = await _loanAdminService.GetAdminByUserIdAsync(userId);
            if (admin == null)
            {
                return NotFound($"LoanAdmin for UserId {userId} not found.");
            }
            return Ok(admin);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromBody] LoanAdmin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _loanAdminService.CreateAdminAsync(admin);
            if (!result)
            {
                return StatusCode(500, "A problem occurred while creating the admin.");
            }

            return CreatedAtAction(nameof(GetAdminById), new { id = admin.AdminId }, admin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, [FromBody] LoanAdmin admin)
        {
            if (id != admin.AdminId)
            {
                return BadRequest("ID in URL does not match ID in request body.");
            }

            var existingAdmin = await _loanAdminService.GetAdminByIdAsync(id);
            if (existingAdmin == null)
            {
                return NotFound($"LoanAdmin with ID {id} not found.");
            }

            var result = await _loanAdminService.UpdateAdminAsync(admin);
            if (!result)
            {
                return StatusCode(500, "A problem occurred while updating the admin.");
            }

            return NoContent();
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetSystemStatistics()
        {
            var stats = await _loanAdminService.GetSystemStatisticsAsync();
            return Ok(stats);
        }

        [HttpGet("pending-applications")]
        public async Task<IActionResult> GetPendingApplications()
        {
            var pendingApplications = await _loanAdminService.GetPendingApplicationsForReviewAsync();
            return Ok(pendingApplications);
        }

        [HttpGet("overdue-loans")]
        public async Task<IActionResult> GetOverdueLoans()
        {
            var overdueLoans = await _loanAdminService.GetOverdueLoansReportAsync();
            return Ok(overdueLoans);
        }

        [HttpGet("npa-loans")]
        public async Task<IActionResult> GetNPALoans()
        {
            var npaLoans = await _loanAdminService.GetNPALoansReportAsync();
            return Ok(npaLoans);
        }

        [HttpPost("{adminId}/reports/collection")]
        public async Task<IActionResult> GenerateCollectionReport(int adminId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var report = await _loanAdminService.GenerateCollectionReportAsync(adminId, startDate, endDate);
            if (report == null)
            {
                return StatusCode(500, "A problem occurred while generating the report.");
            }
            return Ok(report);
        }

        [HttpPost("{adminId}/reports/performance")]
        public async Task<IActionResult> GenerateLoanPerformanceReport(int adminId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var report = await _loanAdminService.GenerateLoanPerformanceReportAsync(adminId, startDate, endDate);
            if (report == null)
            {
                return StatusCode(500, "A problem occurred while generating the report.");
            }
            return Ok(report);
        }

        [HttpGet("{adminId}/reports")]
        public async Task<IActionResult> GetAdminReports(int adminId)
        {
            var reports = await _loanAdminService.GetAdminReportsHistoryAsync(adminId);
            return Ok(reports);
        }
    }
}