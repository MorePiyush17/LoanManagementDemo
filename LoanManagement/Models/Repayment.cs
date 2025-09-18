namespace LoanManagement.Models
{
    public class Repayment
    {
        public int RepaymentId { get; set; }
        public int InstallmentId { get; set; }
        public double AmountPaid { get; set; }
        public DateTime PayDate { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        
        //Navigation Property
        public Installment Installment { get; set; }
        //One installment many repayment
    }
}
