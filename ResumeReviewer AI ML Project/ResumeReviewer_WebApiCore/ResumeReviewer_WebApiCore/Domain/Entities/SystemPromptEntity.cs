namespace ResumeReviewer_WebApiCore.Domain.Entities
{
    public class SystemPromptEntity
    {
        public int PromptId { get; set; }
        public string ExperienceLevel { get; set; } = string.Empty; // Fresher, Junior, Senior
        public string Domain { get; set; } = "Any"; // Optional: Web, ML, etc.
        public string Content { get; set; } = string.Empty; // The actual system message
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
