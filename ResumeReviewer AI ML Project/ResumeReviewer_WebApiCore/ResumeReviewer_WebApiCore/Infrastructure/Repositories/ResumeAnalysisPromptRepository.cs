using Dapper;
using Microsoft.Data.SqlClient;
using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Domain.Entities;

namespace ResumeReviewer_WebApiCore.Infrastructure.Repositories
{
    public class ResumeAnalysisPromptRepository : IResumeAnalysisPromptRepository
    {
        private readonly string _connectionString;
        public ResumeAnalysisPromptRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        public async Task<List<ResumeAnalysisPromptEntity>> GetAllAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sqlQuery = "SELECT PromptId,Category,Content,Embedding FROM ResumeAnalysisPrompts";

                var prompts = await connection.QueryAsync<ResumeAnalysisPromptEntity>(sqlQuery);

                return prompts.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving resume analysis prompts", ex);
            }
        }

        // Get prompts by category: Strength, Weakness, Score
        public async Task<List<ResumeAnalysisPromptEntity>> GetByCategoryAsync(string category)
        {
            try
            {
                const string sqlQuery = @"  SELECT *
                                        FROM ResumeAnalysisPrompts
                                        WHERE Category = @Category";

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var result = await connection.QueryAsync<ResumeAnalysisPromptEntity>(sqlQuery, new { Category = category });

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving prompts for category: {category}", ex);
            }
        }

        public async Task<int> SaveAsync(ResumeAnalysisPromptEntity prompt)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var sql = @"INSERT INTO ResumeAnalysisPrompts (Category, Content, Embedding)
                            OUTPUT INSERTED.PromptId
                            VALUES (@Category, @Content, @Embedding)";

                return await connection.ExecuteScalarAsync<int>(sql, prompt);
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving resume analysis prompt", ex);
            }
        }

    }
}
