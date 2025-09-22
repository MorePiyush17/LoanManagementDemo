using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoanManagement.Models
{
    public class LoanScheme
    {
        [Key]
        public int SchemeId { get; set; }

        [Required(ErrorMessage = "Scheme name is required")]
        public string SchemeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Interest rate is required")]
        [Range(0.01, 100.0, ErrorMessage = "Interest rate must be between 0.01 and 100")]
        public decimal InterestRate { get; set; }

        [Required(ErrorMessage = "Maximum loan amount is required")]
        [Range(1000, double.MaxValue, ErrorMessage = "Maximum amount must be at least 1000")]
        public decimal MaxAmount { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 360, ErrorMessage = "Duration must be between 1 and 360 months")]
        public int DurationsInMonths { get; set; }

        public string Description { get; set; } = string.Empty;

        // Navigation property
        public ICollection<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();
    }
}
