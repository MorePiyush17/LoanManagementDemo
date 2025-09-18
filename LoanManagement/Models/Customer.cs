namespace LoanManagement.Models
{
    public class Customer
    {
        public int CustomerId {  get; set; }
        public int UserId { get; set; } 
        public string City { get; set; }    
        public int ContactNumber { get; set; }

        //navigationproperty
        public User User { get; set; }
        public ICollection<LoanApplication>? LoanApplications { get; set; }

    }
}
