using ResumeReviewer_WebApiCore.Domain.Entities;

namespace ResumeReviewer_WebApiCore.Application.Interfaces
{
    public interface IResumeAnalysisPromptRepository
    {
        Task<List<ResumeAnalysisPromptEntity>> GetAllAsync();

        Task<List<ResumeAnalysisPromptEntity>> GetByCategoryAsync(string category);

        Task<int> SaveAsync(ResumeAnalysisPromptEntity prompt);
    }
}
