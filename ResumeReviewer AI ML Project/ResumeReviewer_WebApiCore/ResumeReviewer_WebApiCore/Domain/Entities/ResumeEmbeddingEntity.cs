namespace ResumeReviewer_WebApiCore.Domain.Entities
{
    public class ResumeEmbeddingEntity
    {
        public int EmbeddingId { get; set; }

        public int ChunkId { get; set; }
        public byte[] Vector { get; set; } = Array.Empty<byte>();
    }
}
