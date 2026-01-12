using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Domain.Entities;

namespace ResumeReviewer_WebApiCore.Infrastructure.Repositories
{
    public class SystemPromptRepository : ISystemPromptRepository
    {
        private readonly string _connectionString;

        public SystemPromptRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<SystemPromptEntity> GetPromptByExperienceAsync(int yearsOfExperience)
        {
            try
            {
                var experienceLevel = MapYearsToExperienceLevel(yearsOfExperience);

                const string sqlQuery = @" SELECT TOP 1 * FROM SystemPrompts
                                           WHERE ExperienceLevel = @ExperienceLevel
                                                 AND Domain = 'Any'
                                           ORDER BY CreatedAt DESC";

                using var connection = new SqlConnection(_connectionString);

                var prompt = await connection.QueryFirstOrDefaultAsync<SystemPromptEntity>(
                            sqlQuery, new { ExperienceLevel = experienceLevel });

                // Fallback to generic if no prompt found
                if (prompt == null)
                {
                    const string fallbackQuery = @"  SELECT TOP 1 * FROM SystemPrompts
                                                     WHERE Domain = 'Any'
                                                     ORDER BY CreatedAt DESC";

                    prompt = await connection.QueryFirstOrDefaultAsync<SystemPromptEntity>(fallbackQuery);
                }

                return prompt;
            }
            catch (Exception ex)
            {
              throw;
            }
        }

        private static string MapYearsToExperienceLevel(int years)
        {
            if (years <= 1)
                return "Fresher";

            if (years <= 2)
                return "Junior";

            if (years <= 5)
                return "Mid-level";

            return "Senior";
        }
    }
}
