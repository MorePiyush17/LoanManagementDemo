namespace LoanManagement.Models
{
    public class LoanAdmin
    {
        public int AdminId { get; set; }
        public int UserId { get; set; } 

        //navigationprop
        public User? User    { get; set; }
        public <ICollection<Report> Reports { get; set; }   

    }
}
