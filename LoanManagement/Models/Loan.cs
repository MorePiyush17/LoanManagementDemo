namespace LoanManagement.Models
{
    public class Loan
    {
        public int LoanId { get; set; }
        public int ApplicationId { get; set; }
        public double LoanAmount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsNPA { get; set; }

        // Navigation properties
        public LoanApplication LoanApplication { get; set; }
        public ICollection<Installment> Installments { get; set; } //one loan many installments
    }

}
