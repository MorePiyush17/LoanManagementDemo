using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using LoanManagement.Services.Interface;

namespace LoanManagement.Services.Implementation
{
    public class LoanApplicationService : ILoanApplicationService
    {
        private readonly ILoanApplicationRepository _applicationRepo;
        private readonly IDocumentRepository _documentRepo;
        private readonly ILoanOfficerService _officerService;

        public LoanApplicationService(ILoanApplicationRepository applicationRepo, IDocumentRepository documentRepo,
                                    ILoanOfficerService officerService)
        {
            _applicationRepo = applicationRepo;
            _documentRepo = documentRepo;
            _officerService = officerService;
        }

        public async Task<int> SubmitApplicationAsync(LoanApplication application)
        {
            application.ApplicationDate = DateTime.Now;
            application.Status = "Pending";

            await _applicationRepo.AddApplicationAsync(application);
            if (await _applicationRepo.SaveAsync())
                return application.ApplicationId;

            return 0;
        }
        public async Task<bool> UploadDocumentsAsync(int applicationId, IEnumerable<Document> documents)
        {
            foreach (var document in documents)
            {
                document.ApplicationId = applicationId;
                document.UploadDate = DateTime.Now;
                await _documentRepo.AddDocumentAsync(document);
            }
            return await _documentRepo.SaveAsync();
        }
        public async Task<LoanApplication?> GetApplicationDetailsAsync(int applicationId)
        {
            return await _applicationRepo.GetApplicationWithDetailsAsync(applicationId);
        }

        public async Task<IEnumerable<LoanApplication>> GetPendingApplicationsAsync()
        {
            return await _applicationRepo.GetPendingApplicationsAsync();
        }
        public async Task<IEnumerable<LoanApplication>> GetApplicationsByStatusAsync(string status)
        {
            return await _applicationRepo.GetApplicationsByStatusAsync(status);
        }

        public async Task<bool> AssignApplicationToOfficerAsync(int applicationId, int officerId)
        {
            var application = await _applicationRepo.GetByIdAsync(applicationId);
            if (application == null) return false;

            application.AssignedOfficerId = officerId;
            _applicationRepo.UpdateApplication(application);
            return await _applicationRepo.SaveAsync();
        }

        public async Task<bool> UpdateApplicationStatusAsync(int applicationId, string status, string remarks)
        {
            var application = await _applicationRepo.GetByIdAsync(applicationId);
            if (application == null) return false;

            application.Status = status;
            _applicationRepo.UpdateApplication(application);
            return await _applicationRepo.SaveAsync();
        }

        public async Task<bool> ValidateApplicationEligibilityAsync(LoanApplication application)
        {
            return application.LoanAmount > 0 &&
                   application.RequestTenureInMonths > 0 &&
                   !string.IsNullOrEmpty(application.DocumentUploaded);
        }
        public async Task<bool> VerifyDocumentCompletenessAsync(int applicationId)
        {
            var documents = await _documentRepo.GetDocumentsByApplicationAsync(applicationId);
            var requiredDocs = new[] { "Identity Proof", "Address Proof", "Income Proof", "Bank Statement" };

            return requiredDocs.All(rd => documents.Any(d => d.FileName.Contains(rd, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<IEnumerable<string>> GetMissingDocumentsAsync(int applicationId)
        {
            var documents = await _documentRepo.GetDocumentsByApplicationAsync(applicationId);
            var requiredDocs = new[] { "Identity Proof", "Address Proof", "Income Proof", "Bank Statement" };

            return requiredDocs.Where(rd => !documents.Any(d => d.FileName.Contains(rd, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<IEnumerable<LoanApplication>> GetCustomerApplicationsAsync(int customerId)
        {
            return await _applicationRepo.GetApplicationsByCustomerAsync(customerId);
        }
    }
}
