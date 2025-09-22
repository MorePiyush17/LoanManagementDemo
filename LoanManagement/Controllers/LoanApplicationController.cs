using LoanManagement.Models;
using LoanManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanApplicationsController : ControllerBase
    {
        private readonly ILoanApplicationService _loanApplicationService;

        public LoanApplicationsController(ILoanApplicationService loanApplicationService)
        {
            _loanApplicationService = loanApplicationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationDetails(int id)
        {
            var application = await _loanApplicationService.GetApplicationDetailsAsync(id);
            if (application == null)
            {
                return NotFound($"Loan application with ID {id} not found.");
            }
            return Ok(application);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerApplications(int customerId)
        {
            var applications = await _loanApplicationService.GetCustomerApplicationsAsync(customerId);
            return Ok(applications);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetApplicationsByStatus(string status)
        {
            var applications = await _loanApplicationService.GetApplicationsByStatusAsync(status);
            return Ok(applications);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingApplications()
        {
            var applications = await _loanApplicationService.GetPendingApplicationsAsync();
            return Ok(applications);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitApplication([FromBody] LoanApplication application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationId = await _loanApplicationService.SubmitApplicationAsync(application);
            if (applicationId == 0)
            {
                return StatusCode(500, "A problem occurred while submitting the application.");
            }

            return CreatedAtAction(nameof(GetApplicationDetails), new { id = applicationId }, new { ApplicationId = applicationId });
        }

        [HttpPost("{applicationId}/documents")]
        public async Task<IActionResult> UploadDocuments(int applicationId, [FromBody] IEnumerable<Document> documents)
        {
            if (!documents.Any())
            {
                return BadRequest("No documents were provided for upload.");
            }

            var result = await _loanApplicationService.UploadDocumentsAsync(applicationId, documents);
            if (!result)
            {
                return StatusCode(500, "A problem occurred while uploading documents.");
            }

            return Ok("Documents uploaded successfully.");
        }

        [HttpPut("{applicationId}/assign/{officerId}")]
        public async Task<IActionResult> AssignApplicationToOfficer(int applicationId, int officerId)
        {
            var result = await _loanApplicationService.AssignApplicationToOfficerAsync(applicationId, officerId);
            if (!result)
            {
                return NotFound($"Loan application with ID {applicationId} or officer with ID {officerId} not found, or an error occurred during assignment.");
            }
            return NoContent();
        }

        [HttpPut("{applicationId}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId, [FromQuery] string status, [FromQuery] string remarks)
        {
            var result = await _loanApplicationService.UpdateApplicationStatusAsync(applicationId, status, remarks);
            if (!result)
            {
                return NotFound($"Loan application with ID {applicationId} not found or an error occurred while updating.");
            }
            return NoContent();
        }

        [HttpGet("{applicationId}/documents/missing")]
        public async Task<IActionResult> GetMissingDocuments(int applicationId)
        {
            var missingDocuments = await _loanApplicationService.GetMissingDocumentsAsync(applicationId);
            if (missingDocuments == null)
            {
                return NotFound($"Loan application with ID {applicationId} not found.");
            }
            return Ok(missingDocuments);
        }
    }
}