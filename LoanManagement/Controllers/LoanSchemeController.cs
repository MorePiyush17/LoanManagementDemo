using LoanManagement.Models;
using LoanManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanSchemesController : ControllerBase
    {
        private readonly ILoanSchemeService _loanSchemeService;

        public LoanSchemesController(ILoanSchemeService loanSchemeService)
        {
            _loanSchemeService = loanSchemeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchemeById(int id)
        {
            var scheme = await _loanSchemeService.GetSchemeByIdAsync(id);
            if (scheme == null)
            {
                return NotFound($"Loan scheme with ID {id} not found.");
            }
            return Ok(scheme);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSchemes()
        {
            var schemes = await _loanSchemeService.GetAllActiveSchemesAsync();
            return Ok(schemes);
        }

        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommendedSchemes([FromQuery] decimal amount, [FromQuery] int preferredTenure)
        {
            var schemes = await _loanSchemeService.GetRecommendedSchemesAsync(amount, preferredTenure);
            return Ok(schemes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheme([FromBody] LoanScheme scheme)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _loanSchemeService.CreateSchemeAsync(scheme);
            if (!result)
            {
                return StatusCode(500, "A problem occurred while creating the loan scheme.");
            }

            return CreatedAtAction(nameof(GetSchemeById), new { id = scheme.SchemeId }, scheme);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScheme(int id, [FromBody] LoanScheme scheme)
        {
            if (id != scheme.SchemeId)
            {
                return BadRequest("ID in URL does not match ID in request body.");
            }

            var existingScheme = await _loanSchemeService.GetSchemeByIdAsync(id);
            if (existingScheme == null)
            {
                return NotFound($"Loan scheme with ID {id} not found.");
            }

            var result = await _loanSchemeService.UpdateSchemeAsync(scheme);
            if (!result)
            {
                return StatusCode(500, "A problem occurred while updating the loan scheme.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateScheme(int id)
        {
            var result = await _loanSchemeService.DeactivateSchemeAsync(id);
            if (!result)
            {
                return NotFound($"Loan scheme with ID {id} not found or an error occurred during deactivation.");
            }
            return NoContent();
        }

        [HttpGet("{schemeId}/emi")]
        public async Task<IActionResult> CalculateEMI(int schemeId, [FromQuery] decimal loanAmount, [FromQuery] int tenureMonths)
        {
            var emi = await _loanSchemeService.CalculateEMIAsync(schemeId, loanAmount, tenureMonths);
            if (emi == 0)
            {
                return NotFound($"Loan scheme with ID {schemeId} not found or invalid calculation parameters.");
            }
            return Ok(new { EMI = emi });
        }

        [HttpGet("{schemeId}/total-interest")]
        public async Task<IActionResult> CalculateTotalInterest(int schemeId, [FromQuery] decimal loanAmount, [FromQuery] int tenureMonths)
        {
            var totalInterest = await _loanSchemeService.CalculateTotalInterestAsync(schemeId, loanAmount, tenureMonths);
            if (totalInterest == 0)
            {
                return NotFound($"Loan scheme with ID {schemeId} not found or invalid calculation parameters.");
            }
            return Ok(new { TotalInterest = totalInterest });
        }

        [HttpGet("{schemeId}/validate-amount")]
        public async Task<IActionResult> IsAmountValidForScheme(int schemeId, [FromQuery] decimal amount)
        {
            var isValid = await _loanSchemeService.IsAmountValidForSchemeAsync(schemeId, amount);
            return Ok(new { IsValid = isValid });
        }
    }
}