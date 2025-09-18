namespace LoanManagement.Models
{
    public class Installment
    {
        public int InstallmentId { get; set; }
        public int LoanId { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal AmountDue { get; set; }
        public decimal AmountPaid { get; set; }
        public string Status { get; set; }

        // Navigation property
        public Loan Loan { get; set; }
        public ICollection<Repayment> Repayments { get; set; }
       //one installments many repayments
    }
}
