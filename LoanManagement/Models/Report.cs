using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagement.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        [ForeignKey(nameof(LoanAdmin))]
        public int AdminId { get; set; }

        [Required]
        [StringLength(100)]
        public string ReportType { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public DateTime GeneratedDate { get; set; } = DateTime.Now;

        // Navigation Property
        public LoanAdmin? LoanAdmin { get; set; }
    }
}