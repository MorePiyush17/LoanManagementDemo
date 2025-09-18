namespace LoanManagement.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public int AdminId { get; set; }
        public string ReportType { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }

    }
}
