namespace ResumeReviewer_WebApiCore.Domain.Entities
{
    public class ResumeChunkEntity
    {
        public int ChunkId { get; set; }
        public int ResumeId { get; set; }
        public string ChunkText { get; set; }             
        public DateTime CreatedAt { get; set; }
    }
}
