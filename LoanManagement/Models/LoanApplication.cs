using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace LoanManagement.Models
{
    public class LoanApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "CustomerId is required")]
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Assigned OfficerId is required")]
        [ForeignKey(nameof(LoanOfficer))]
        public int AssignedOfficerId { get; set; }

        [Required(ErrorMessage = "SchemeId is required")]
        [ForeignKey(nameof(LoanScheme))]
        public int SchemeId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Default: Pending, Approved, Rejected

        [Required(ErrorMessage = "Application Date is required")]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Loan amount is required")]
        [Range(1000, double.MaxValue, ErrorMessage = "Loan amount must be at least 1000")]
        public decimal LoanAmount { get; set; }

        [Required(ErrorMessage = "Tenure is required")]
        [Range(1, 360, ErrorMessage = "Tenure must be between 1 and 360 months")]
        public int RequestTenureInMonths { get; set; }

        public string DocumentUploaded { get; set; } = string.Empty; // Store filename/path

        // Navigation Properties
        public Customer? Customer { get; set; }
        public LoanOfficer? LoanOfficer { get; set; }
        public LoanScheme? LoanScheme { get; set; }
        public Loan? Loan { get; set; } // One-to-One: Created only if approved
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
