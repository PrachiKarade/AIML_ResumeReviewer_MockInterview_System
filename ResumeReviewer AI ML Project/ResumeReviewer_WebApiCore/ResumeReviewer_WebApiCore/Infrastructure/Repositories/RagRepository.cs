using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Domain.Entities;
using ResumeReviewer_WebApiCore.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using ResumeReviewer_WebApiCore.Application.Services;

namespace ResumeReviewer_WebApiCore.Infrastructure.Repositories
{
    public class RagRepository :IRagRepository
    {
        private readonly string _connectionString;

        public RagRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<List<ResumeChunkEntity>> GetChunksByResumeIdAsync(int resumeId)
        {
           try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                var sql = @"SELECT ChunkId, ResumeId, ChunkText, CreatedAt
                            FROM ResumeChunks
                            WHERE ResumeId = @ResumeId
                            ORDER BY CreatedAt ASC";

                var chunks = await connection.QueryAsync<ResumeChunkEntity>(sql, new { ResumeId = resumeId });

                return chunks.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ResumeEmbeddingEntity>> GetEmbeddingsByChunksAsync(List<int> chunkIds)
        {
            if (chunkIds == null || !chunkIds.Any())
                return new List<ResumeEmbeddingEntity>();

            try
            {
                using var connection = new SqlConnection(_connectionString);

                var sql = @" SELECT EmbeddingId, ChunkId, Vector
                             FROM ResumeEmbeddings
                             WHERE ChunkId IN @ChunkIds";

                var embeddings = await connection.QueryAsync<ResumeEmbeddingEntity>(sql, new { ChunkIds = chunkIds });

                return embeddings.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching chunk embeddings", ex);
            }
        }

        public async Task StoreChunkAsync(ResumeChunkEntity chunk, float[] embedding)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var chunkId = await connection.ExecuteScalarAsync<int>(
                    @"INSERT INTO ResumeChunks (ResumeId, ChunkText, CreatedAt)
                      OUTPUT INSERTED.ChunkId
                      VALUES (@ResumeId, @ChunkText, @CreatedAt)", chunk);

                var vectorBytes = CosineSimilarityService.FloatArrayToBytes(embedding); // convert float embeddings vector to byte array

                await connection.ExecuteAsync(
                    @"INSERT INTO ResumeEmbeddings (ChunkId, Vector)
                      VALUES (@ChunkId, @Vector)",
                    new { ChunkId = chunkId, Vector = vectorBytes });
            }
            catch (Exception ex)
            {
                // Log exception (not implemented here)
                throw;
            }
        }

        public async Task<List<ResumeChunkModel>> SearchSimilarChunksAsync(float[] queryEmbedding, int topK)
        {
            try
            { 
                // Load all embeddings from DB
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var sql = @"
                        SELECT e.EmbeddingId, e.ChunkId, e.Vector, c.ChunkText
                        FROM ResumeEmbeddings e
                        INNER JOIN ResumeChunks c 
                           ON c.ChunkId = e.ChunkId";

                var results = await connection.QueryAsync<(int EmbeddingId, int ChunkId, byte[] Vector, string ChunkText)>(sql);

                // Convert byte[] → float[] and compute similarity
                var scoredChunks = results.Select(r =>
                {
                    var vector = CosineSimilarityService.BytesToFloatArray(r.Vector);
                    var score = CosineSimilarityService.CosineSimilarity(vector, queryEmbedding);
                    return new { Chunk = new ResumeChunkModel { ChunkId = r.ChunkId, ChunkText = r.ChunkText }, Score = score };
                })
                .OrderByDescending(x => x.Score)
                .Take(topK)
                .Select(x => x.Chunk)
                .ToList();

                return scoredChunks;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
           
    }
}
