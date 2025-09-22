using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using LoanManagement.Models;
using LoanManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] DocumentUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("File not provided.");
            }

            // In a real app, you would save the file to a secure location and get the path.
            // For this example, we'll simulate the process.
            var filePath = Path.Combine("Uploads", request.File.FileName);
            using (var stream = new MemoryStream())
            {
                await request.File.CopyToAsync(stream);
                var fileContent = stream.ToArray();

                var document = new Document
                {
                    ApplicationId = request.ApplicationId,
                    FileName = request.File.FileName,
                    FilePath = filePath
                };

                var documentId = await _documentService.UploadDocumentAsync(document, fileContent);

                if (documentId == 0)
                {
                    return StatusCode(500, "A problem occurred while uploading the document.");
                }

                // Return details of the created document with a 201 Created status
                return CreatedAtAction(nameof(GetDocumentDetails), new { id = documentId }, new { DocumentId = documentId, FileName = document.FileName });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentDetails(int id)
        {
            var document = await _documentService.GetDocumentDetailsAsync(id);
            if (document == null)
            {
                return NotFound($"Document with ID {id} not found.");
            }
            return Ok(document);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadDocument(int id)
        {
            var fileContent = await _documentService.DownloadDocumentAsync(id);
            if (fileContent == null)
            {
                return NotFound($"Document with ID {id} not found or file does not exist.");
            }

            var document = await _documentService.GetDocumentDetailsAsync(id);
            if (document == null)
            {
                return NotFound($"Document details with ID {id} not found.");
            }

            return File(fileContent, "application/octet-stream", document.FileName);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var result = await _documentService.DeleteDocumentAsync(id);
            if (!result)
            {
                return NotFound($"Document with ID {id} not found.");
            }
            return NoContent();
        }

        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetApplicationDocuments(int applicationId)
        {
            var documents = await _documentService.GetApplicationDocumentsAsync(applicationId);
            return Ok(documents);
        }

        [HttpGet("application/{applicationId}/verify-completeness")]
        public async Task<IActionResult> VerifyDocumentCompleteness(int applicationId)
        {
            var isComplete = await _documentService.VerifyDocumentCompletenessAsync(applicationId);
            return Ok(new { ApplicationId = applicationId, IsComplete = isComplete });
        }

        [HttpGet("application/{applicationId}/missing-types")]
        public async Task<IActionResult> GetMissingDocumentTypes(int applicationId)
        {
            var missingTypes = await _documentService.GetMissingDocumentTypesAsync(applicationId);
            return Ok(missingTypes);
        }

        [HttpGet("{id}/validate")]
        public async Task<IActionResult> ValidateDocument(int id)
        {
            var isValid = await _documentService.ValidateDocumentAsync(id);
            return Ok(new { DocumentId = id, IsValid = isValid });
        }

        [HttpGet("is-required")]
        public async Task<IActionResult> IsDocumentTypeRequired([FromQuery] string documentType, [FromQuery] int schemeId)
        {
            var isRequired = await _documentService.IsDocumentTypeRequiredAsync(documentType, schemeId);
            return Ok(new { DocumentType = documentType, SchemeId = schemeId, IsRequired = isRequired });
        }
    }

    public class DocumentUploadRequest
    {
        [Required]
        public int ApplicationId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}