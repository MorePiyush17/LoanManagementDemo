using LoanManagement.Models;
using LoanManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanOfficersController : ControllerBase
    {
        private readonly ILoanOfficerService _loanOfficerService;

        public LoanOfficersController(ILoanOfficerService loanOfficerService)
        {
            _loanOfficerService = loanOfficerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOfficerById(int id)
        {
            var officer = await _loanOfficerService.GetOfficerByIdAsync(id);
            if (officer == null)
            {
                return NotFound($"LoanOfficer with ID {id} not found.");
            }
            return Ok(officer);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOfficerByUserId(int userId)
        {
            var officer = await _loanOfficerService.GetOfficerByUserIdAsync(userId);
            if (officer == null)
            {
                return NotFound($"LoanOfficer for UserId {userId} not found.");
            }
            return Ok(officer);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOfficers()
        {
            var officers = await _loanOfficerService.GetAllOfficersAsync();
            return Ok(officers);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOfficer([FromBody] LoanOfficer officer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _loanOfficerService.CreateOfficerAsync(officer);
            if (!result)
            {
                return StatusCode(500, "A problem occurred while creating the loan officer.");
            }

            return CreatedAtAction(nameof(GetOfficerById), new { id = officer.OfficerId }, officer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOfficer(int id, [FromBody] LoanOfficer officer)
        {
            if (id != officer.OfficerId)
            {
                return BadRequest("ID in URL does not match ID in request body.");
            }

            var existingOfficer = await _loanOfficerService.GetOfficerByIdAsync(id);
            if (existingOfficer == null)
            {
                return NotFound($"LoanOfficer with ID {id} not found.");
            }

            var result = await _loanOfficerService.UpdateOfficerAsync(officer);
            if (!result)
            {
                return StatusCode(500, "A problem occurred while updating the loan officer.");
            }

            return NoContent();
        }

        [HttpGet("{officerId}/workqueue")]
        public async Task<IActionResult> GetOfficerWorkqueue(int officerId)
        {
            var applications = await _loanOfficerService.GetOfficerWorkqueueAsync(officerId);
            return Ok(applications);
        }

        [HttpGet("{officerId}/workload")]
        public async Task<IActionResult> GetOfficerWorkload(int officerId)
        {
            var workload = await _loanOfficerService.GetOfficerWorkloadAsync(officerId);
            return Ok(new { OfficerId = officerId, Workload = workload });
        }

        [HttpGet("least-busy/{city}")]
        public async Task<IActionResult> GetLeastBusyOfficerInCity(string city)
        {
            var officer = await _loanOfficerService.GetLeastBusyOfficerInCityAsync(city);
            if (officer == null)
            {
                return NotFound($"No officers found in the city: {city}.");
            }
            return Ok(officer);
        }

        [HttpGet("applications/{applicationId}")]
        public async Task<IActionResult> GetApplicationForReview(int applicationId)
        {
            var application = await _loanOfficerService.GetApplicationForReviewAsync(applicationId);
            if (application == null)
            {
                return NotFound($"Loan application with ID {applicationId} not found.");
            }
            return Ok(application);
        }

        [HttpPost("applications/{applicationId}/process")]
        public async Task<IActionResult> ProcessLoanApplication(int applicationId, [FromQuery] string decision, [FromQuery] string remarks)
        {
            if (string.IsNullOrEmpty(decision))
            {
                return BadRequest("Decision must be 'Approved' or 'Rejected'.");
            }

            var result = await _loanOfficerService.ProcessLoanApplicationAsync(applicationId, decision, remarks);
            if (!result)
            {
                return StatusCode(500, $"A problem occurred while processing application {applicationId}.");
            }

            return Ok($"Application {applicationId} has been {decision}.");
        }

        [HttpPost("applications/{applicationId}/request-documents")]
        public async Task<IActionResult> RequestAdditionalDocuments(int applicationId, [FromQuery] string documentList)
        {
            if (string.IsNullOrEmpty(documentList))
            {
                return BadRequest("Document list cannot be empty.");
            }

            var result = await _loanOfficerService.RequestAdditionalDocumentsAsync(applicationId, documentList);
            if (!result)
            {
                return StatusCode(500, $"A problem occurred while requesting documents for application {applicationId}.");
            }

            return Ok($"Additional documents requested for application {applicationId}.");
        }
    }
}