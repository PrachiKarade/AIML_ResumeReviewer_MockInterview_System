namespace ResumeReviewer_WebApiCore.Domain.Models
{
    public class ResumeChunkModel
    {
        public int ChunkId { get; set; }
        //public int ResumeId { get; set; }
        public string ChunkText { get; set; }
        //public DateTime CreatedAt { get; set; }
        public float SimilarityScore { get; set; }
    }
}
