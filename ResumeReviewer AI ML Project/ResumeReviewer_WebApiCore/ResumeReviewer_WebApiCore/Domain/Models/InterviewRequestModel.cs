namespace ResumeReviewer_WebApiCore.Domain.Models
{
    public class InterviewRequestModel
    {
        public int ResumeId { get; set; }

        public string? UserAnswer { get; set; } = string.Empty;
    }
}
