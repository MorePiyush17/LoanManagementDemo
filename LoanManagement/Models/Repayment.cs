using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagement.Models
{
    public class Repayment
    {
        [Key]
        public int RepaymentId { get; set; }

        [Required]
        [ForeignKey(nameof(Installment))]
        public int InstallmentId { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Repayment amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime RepaymentDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Method { get; set; } = string.Empty; // Cash, Online, Cheque, etc.

        [StringLength(100)]
        public string TransactionId { get; set; } = string.Empty;

        // Navigation Properties
        public Installment? Installment { get; set; }
        public Customer? Customer { get; set; }
    }
}
