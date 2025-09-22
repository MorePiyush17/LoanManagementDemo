using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagement.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^[6-9]\d{9}$",
            ErrorMessage = "Contact number must be a valid 10-digit number")]
        public string ContactNumber { get; set; } = string.Empty;

        // Navigation Properties
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<Installment> Installments { get; set; } = new List<Installment>();
        public ICollection<Repayment> Repayments { get; set; } = new List<Repayment>();
    }
}
