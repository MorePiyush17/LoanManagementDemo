using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagement.Models
{
    public class LoanAdmin
    {
        [Key]
        public int AdminId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        // Navigation Properties
        public User? User { get; set; }
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}