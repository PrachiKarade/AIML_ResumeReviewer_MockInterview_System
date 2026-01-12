namespace ResumeReviewer_WebApiCore.Domain.Models
{
    public class ResumeAnalysisModel
    {
        // The ID of the resume being analyzed
        public int ResumeId { get; set; }

        // Overall score of the resume (e.g., 0-100)
        public int Score { get; set; }

        // Experience level inferred from the resume
        public string ExperienceLevel { get; set; }

        // Strengths identified in the resume (e.g., good projects, skills)
        public List<string> Strengths { get; set; } = new List<string>();

        // Areas for improvement based on experience, keywords, formatting
        public List<string> Improvements { get; set; } = new List<string>();
    }
}
