using LoanManagement.Models;
using LoanManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("generate/portfolio")]
        public async Task<IActionResult> GenerateLoanPortfolioReport([FromQuery] int adminId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be after end date.");
            }
            var report = await _reportService.GenerateLoanPortfolioReportAsync(adminId, startDate, endDate);
            return CreatedAtAction(nameof(GetReport), new { id = report.ReportId }, report);
        }

        [HttpPost("generate/collection")]
        public async Task<IActionResult> GenerateCollectionReport([FromQuery] int adminId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be after end date.");
            }
            var report = await _reportService.GenerateCollectionReportAsync(adminId, startDate, endDate);
            return CreatedAtAction(nameof(GetReport), new { id = report.ReportId }, report);
        }

        [HttpPost("generate/npa")]
        public async Task<IActionResult> GenerateNPAReport([FromQuery] int adminId)
        {
            var report = await _reportService.GenerateNPAReportAsync(adminId);
            return CreatedAtAction(nameof(GetReport), new { id = report.ReportId }, report);
        }

        [HttpPost("generate/customer")]
        public async Task<IActionResult> GenerateCustomerReport([FromQuery] int adminId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be after end date.");
            }
            var report = await _reportService.GenerateCustomerReportAsync(adminId, startDate, endDate);
            return CreatedAtAction(nameof(GetReport), new { id = report.ReportId }, report);
        }

        [HttpGet("dashboard-metrics")]
        public async Task<IActionResult> GetDashboardMetrics()
        {
            var metrics = await _reportService.GetDashboardMetricsAsync();
            return Ok(metrics);
        }

        [HttpGet("loan-trend")]
        public async Task<IActionResult> GetLoanTrendAnalysis([FromQuery] int months)
        {
            if (months <= 0)
            {
                return BadRequest("Number of months must be greater than 0.");
            }
            var trends = await _reportService.GetLoanTrendAnalysisAsync(months);
            return Ok(trends);
        }

        [HttpGet("collection-trend")]
        public async Task<IActionResult> GetCollectionTrendAnalysis([FromQuery] int months)
        {
            if (months <= 0)
            {
                return BadRequest("Number of months must be greater than 0.");
            }
            var trends = await _reportService.GetCollectionTrendAnalysisAsync(months);
            return Ok(trends);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReport(int id)
        {
            var report = await _reportService.GetReportAsync(id);
            if (report == null)
            {
                return NotFound($"Report with ID {id} not found.");
            }
            return Ok(report);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var result = await _reportService.DeleteReportAsync(id);
            if (!result)
            {
                return NotFound($"Report with ID {id} not found.");
            }
            return NoContent();
        }
    }
}