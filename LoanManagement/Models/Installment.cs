using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagement.Models
{
    public class Installment
    {
        [Key]
        public int InstallmentId { get; set; }

        [Required]
        [ForeignKey(nameof(Loan))]
        public int LoanId { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Installment number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Installment number must be greater than 0")]
        public int InstallmentNumber { get; set; }

        [Required(ErrorMessage = "Due Date is required")]
        public DateTime DueDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage = "Amount Due is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Amount Due must be greater than 0")]
        public decimal AmountDue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount Paid cannot be negative")]
        public decimal AmountPaid { get; set; } = 0;

        [Required(ErrorMessage = "Status is required")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Paid, Overdue

        // Navigation Properties
        public Loan? Loan { get; set; }
        public Customer? Customer { get; set; }
        public Repayment? Repayment { get; set; } // One-to-One relationship
    }
}