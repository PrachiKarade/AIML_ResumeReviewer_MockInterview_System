namespace ResumeReviewer_WebApiCore.Domain.Models
{
    public class ResumeModel
    {
        public int ResumeId { get; set; }
        public string FileName { get; set; }
        public string RawText { get; set; }
        public int YearsOfExperience { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
