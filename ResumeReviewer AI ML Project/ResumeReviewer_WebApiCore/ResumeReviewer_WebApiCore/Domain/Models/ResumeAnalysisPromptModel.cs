namespace ResumeReviewer_WebApiCore.Domain.Models
{
    public class ResumeAnalysisPromptModel
    {
        public string Category { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; // The actual prompt content
        public float SimilarityScore { get; set; }
    }
}
