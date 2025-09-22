using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagement.Models
{
    public class LoanOfficer
    {
        [Key]
        public int OfficerId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = string.Empty;

        // Navigation Properties
        public User? User { get; set; }
        public ICollection<LoanApplication> AssignedApplications { get; set; } = new List<LoanApplication>();
    }
}