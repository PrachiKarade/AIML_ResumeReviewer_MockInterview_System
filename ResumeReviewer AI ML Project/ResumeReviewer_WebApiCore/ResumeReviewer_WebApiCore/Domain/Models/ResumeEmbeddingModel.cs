namespace ResumeReviewer_WebApiCore.Domain.Models
{
    public class ResumeEmbeddingModel
    {       
        public int ChunkId { get; set; }
        public float[] Vector { get; set; }        
    }
}
