namespace ResumeReviewer_WebApiCore.Domain.Models
{
    public class InterviewResponseModel
    {
        public int ResumeId { get; set; }
        public string InterviewQuestion { get; set; } = string.Empty;
        //public string Role { get; set; } = "System";        
    }
}
