using ResumeReviewer_WebApiCore.Domain.Models;

namespace ResumeReviewer_WebApiCore.Application.Interfaces
{
    public interface IRagService
    {
        Task ProcessResumeChunksAsync(int resumeId, string rawText);
        Task<List<ResumeChunkModel>> GetRelevantChunksAsync(float[] queryEmbedding, int topK);        
    }
}
