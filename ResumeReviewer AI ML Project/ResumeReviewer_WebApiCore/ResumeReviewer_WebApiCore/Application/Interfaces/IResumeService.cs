using ResumeReviewer_WebApiCore.Domain.Models;

namespace ResumeReviewer_WebApiCore.Application.Interfaces
{
    public interface IResumeService
    {
        Task<int> UploadResumeAsync(IFormFile file);
        Task<ResumeModel> GetResumeByIdAsync(int resumeId);
        Task<ResumeAnalysisModel> AnalyzeResumeAsync(int resumeId);
//        Task ProcessResumeAsync(IFormFile file);
    }
}
