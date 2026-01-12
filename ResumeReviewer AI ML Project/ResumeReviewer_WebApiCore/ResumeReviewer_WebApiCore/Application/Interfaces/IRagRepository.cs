using ResumeReviewer_WebApiCore.Domain.Entities;
using ResumeReviewer_WebApiCore.Domain.Models;

namespace ResumeReviewer_WebApiCore.Application.Interfaces
{
    public interface IRagRepository
    {
        Task <List<ResumeChunkEntity>>GetChunksByResumeIdAsync(int resumeId);
        Task <List<ResumeEmbeddingEntity>> GetEmbeddingsByChunksAsync(List<int> chunkIds);
        Task StoreChunkAsync(ResumeChunkEntity chunk, float[] embedding);
        Task<List<ResumeChunkModel>> SearchSimilarChunksAsync(float[] queryEmbedding, int topK);    
        
    }
}
