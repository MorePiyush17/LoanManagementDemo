using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagement.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        [ForeignKey(nameof(LoanApplication))]
        public int ApplicationId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        // Navigation Property
        public LoanApplication? LoanApplication { get; set; }
    }
}