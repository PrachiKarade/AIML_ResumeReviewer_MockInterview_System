using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Domain.Entities;
using ResumeReviewer_WebApiCore.Domain.Models;

namespace ResumeReviewer_WebApiCore.Application.Services
{
    public class RagService :IRagService
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IRagRepository _ragRepository;

        public RagService(IEmbeddingService embeddingService,
                          IRagRepository ragRepository)
        {
            _embeddingService = embeddingService;
            _ragRepository = ragRepository;
        }

        public async Task ProcessResumeChunksAsync(int resumeId, string rawText)
        {
            var chunks = ChunkText(rawText, 500); // Split into 500-word chunks
            foreach (var chunkText in chunks)
            {
                float[] embedding  = _embeddingService.GenerateEmbedding(chunkText);

                await _ragRepository.StoreChunkAsync(
                    new ResumeChunkEntity { ResumeId = resumeId, ChunkText = chunkText,CreatedAt = DateTime.UtcNow },
                    embedding
                );
            }
        }

        public async Task<List<ResumeChunkModel>> GetRelevantChunksAsync(float[] queryEmbedding, int topK)
        {
            return await _ragRepository.SearchSimilarChunksAsync(queryEmbedding, topK);
        }

        private List<string> ChunkText(string text, int chunkSize)
        {
            // Simple splitting logic
            var words = text.Split(' ');
            var chunks = new List<string>();

            for (int i = 0; i < words.Length; i += chunkSize)
            {
                chunks.Add(string.Join(' ', words.Skip(i).Take(chunkSize)));
            }
            return chunks;
        }

        
    }
}
