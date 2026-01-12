namespace ResumeReviewer_WebApiCore.Domain.Entities
{
    public class ResumeAnalysisPromptEntity
    {
        public int PromptId { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public byte[] Embedding { get; set; }
    }
}
