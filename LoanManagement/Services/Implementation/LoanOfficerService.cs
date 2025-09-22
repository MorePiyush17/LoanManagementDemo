using LoanManagement.Models;
using LoanManagement.Repositories.Interface;
using LoanManagement.Services.Interface;

namespace LoanManagement.Services.Implementation
{
    public class LoanOfficerService : ILoanOfficerService
    {
        private readonly ILoanOfficerRepository _officerRepo;
        private readonly ILoanApplicationRepository _applicationRepo;

        public LoanOfficerService(ILoanOfficerRepository officerRepo, ILoanApplicationRepository applicationRepo)
        {
            _officerRepo = officerRepo;
            _applicationRepo = applicationRepo;
        }

        public async Task<LoanOfficer?> GetOfficerByIdAsync(int officerId)
        {
            return await _officerRepo.GetByIdAsync(officerId);
        }

        public async Task<LoanOfficer?> GetOfficerByUserIdAsync(int userId)
        {
            return await _officerRepo.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<LoanOfficer>> GetAllOfficersAsync()
        {
            return await _officerRepo.GetAllOfficersAsync();
        }

        public async Task<bool> CreateOfficerAsync(LoanOfficer officer)
        {
            await _officerRepo.AddOfficerAsync(officer);
            return await _officerRepo.SaveAsync();
        }

        public async Task<bool> UpdateOfficerAsync(LoanOfficer officer)
        {
            _officerRepo.UpdateOfficer(officer);
            return await _officerRepo.SaveAsync();
        }

        public async Task<IEnumerable<LoanApplication>> GetOfficerWorkqueueAsync(int officerId)
        {
            return await _applicationRepo.GetApplicationsByOfficerAsync(officerId);
        }

        public async Task<int> GetOfficerWorkloadAsync(int officerId)
        {
            return await _officerRepo.GetApplicationCountAsync(officerId);
        }

        public async Task<LoanOfficer?> GetLeastBusyOfficerInCityAsync(string city)
        {
            var officers = await _officerRepo.GetOfficersByCityAsync(city);
            LoanOfficer? leastBusyOfficer = null;
            int minWorkload = int.MaxValue;

            foreach (var officer in officers)
            {
                var workload = await GetOfficerWorkloadAsync(officer.OfficerId);
                if (workload < minWorkload)
                {
                    minWorkload = workload;
                    leastBusyOfficer = officer;
                }
            }

            return leastBusyOfficer;
        }

        public async Task<bool> ProcessLoanApplicationAsync(int applicationId, string decision, string remarks)
        {
            var application = await _applicationRepo.GetByIdAsync(applicationId);
            if (application == null) return false;

            application.Status = decision;
            _applicationRepo.UpdateApplication(application);
            return await _applicationRepo.SaveAsync();
        }

        public async Task<bool> RequestAdditionalDocumentsAsync(int applicationId, string documentList)
        {
            var application = await _applicationRepo.GetByIdAsync(applicationId);
            if (application == null) return false;

            application.DocumentUploaded = documentList;
            _applicationRepo.UpdateApplication(application);
            return await _applicationRepo.SaveAsync();
        }

        public async Task<LoanApplication?> GetApplicationForReviewAsync(int applicationId)
        {
            return await _applicationRepo.GetApplicationWithDetailsAsync(applicationId);
        }
    }
}
