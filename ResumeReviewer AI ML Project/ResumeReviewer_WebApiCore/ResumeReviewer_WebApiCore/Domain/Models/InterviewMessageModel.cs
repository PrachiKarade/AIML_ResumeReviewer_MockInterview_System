namespace ResumeReviewer_WebApiCore.Domain.Models
{
    public class InterviewMessageModel
    {
        public InterviewRole Role { get; set; }
        public string Content { get; set; } = string.Empty;

        public InterviewMessageModel(InterviewRole role, string content)
        {
            Role = role;
            Content = content;
        }
    }
    public enum InterviewRole
    {
        System,
        User,
        Assistant
    }
}
