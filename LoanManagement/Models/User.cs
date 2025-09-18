using System.ComponentModel.DataAnnotations;

namespace LoanManagement.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Email is required")]

        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Navigation property
        public Customer Customer { get; set; }
        public LoanOfficer loanofficer { get; set; }
        public LoanAdmin loanAdmin { get; set; }


      
    }
}
