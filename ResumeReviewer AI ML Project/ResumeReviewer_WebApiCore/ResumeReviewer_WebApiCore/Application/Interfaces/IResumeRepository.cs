using ResumeReviewer_WebApiCore.Domain.Entities;

namespace ResumeReviewer_WebApiCore.Application.Interfaces
{
    public interface IResumeRepository
    {
        Task<int> SaveAsync(ResumeEntity resume);
        Task<ResumeEntity> GetByIdAsync(int resumeId);
        Task<List<ResumeEntity>> GetAllAsync(); // Optional: fetch all resumes
    }
}
