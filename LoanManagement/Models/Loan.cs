using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagement.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }

        [Required(ErrorMessage = "ApplicationId is required")]
        [ForeignKey(nameof(LoanApplication))]
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "CustomerId is required")]
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Loan amount is required")]
        [Range(1000, double.MaxValue, ErrorMessage = "Loan amount must be at least 1000")]
        public decimal LoanAmount { get; set; }

        [Required(ErrorMessage = "Issue date is required")]
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Due date is required")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public bool IsNPA { get; set; } = false; // Non-Performing Asset flag

        // Navigation Properties
        public LoanApplication? LoanApplication { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<Installment> Installments { get; set; } = new List<Installment>();
    }

}
