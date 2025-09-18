namespace LoanManagement.Models
{
    public class LoanScheme
    {
        public int SchemeId { get; set; }   
        public string SchemeName { get; set; }
        public double InterestRate { get; set; }    
        public double MaxAmount { get; set; }
        public int DurationsInMonths { get; set; }
        public string Description { get; set; } 

        //navigation properties
        public ICollection<LoanApplication> LoanApplications { get; set; }

    }
}
