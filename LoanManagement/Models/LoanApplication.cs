using System.Reflection.Metadata;

namespace LoanManagement.Models
{
    public class LoanApplication
    {
        public int ApplicationId { get; set; }
        public int CustomerId { get; set; }
        public int AssignedOfficerId { get; set; }
        public int SchemeId { get; set; }
        public string Status { get; set; }
        public DateTime ApplicationDate { get; set; }
        public double LoanAmount { get; set; }
        public int RequestTenureInMonths { get; set; }
        public string DocumentUploaded { get; set; }
        //navigation prop

        public Customer Customer { get; set; }
        public LoanOfficer LoanOfficer { get; set; }
        public LoanScheme LoanScheme { get; set; }
        public Loan Loan { get; set; }
        public ICollection<Document> Documents { get; set; }




    }
}
