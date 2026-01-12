using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ResumeReviewer_WebApiCore.Infrastructure.Repositories
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly string _connectionString;

        public ResumeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<ResumeEntity>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Resumes";

            var resumes = await connection.QueryAsync<ResumeEntity>(query);

            return resumes.ToList();
        }

        public async Task<ResumeEntity> GetByIdAsync(int resumeId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                await connection.OpenAsync();

                var query = "SELECT * FROM Resumes WHERE ResumeId = @ResumeId";

                return await connection.QuerySingleOrDefaultAsync<ResumeEntity>(query, new { ResumeId = resumeId });
            }
            catch (Exception ex)
            {
                // Log exception (not implemented here)
                throw;
            }
        }

        public async Task<int> SaveAsync(ResumeEntity resume)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = @"
                INSERT INTO Resumes (FileName, RawText, YearsOfExperience, CreatedAt)
                OUTPUT INSERTED.ResumeId
                VALUES (@FileName, @RawText, @YearsOfExperience, @CreatedAt)";

                return await connection.ExecuteScalarAsync<int>(query, resume);
            }
            catch (Exception ex)
            {
                // Log exception (not implemented here)
                throw;
            }
        }
    }
}
