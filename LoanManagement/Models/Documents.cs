namespace LoanManagement.Models
{
    public class Documents
    {
        public int DocumentId { get; set; }
        public int ApplicationId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
       //one application many documents
    }
}
