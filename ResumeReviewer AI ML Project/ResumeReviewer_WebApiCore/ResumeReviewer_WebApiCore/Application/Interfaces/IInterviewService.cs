using ResumeReviewer_WebApiCore.Domain.Models;

namespace ResumeReviewer_WebApiCore.Application.Interfaces
{
    public interface IInterviewService
    {
        Task<InterviewResponseModel> StartInterviewAsync(int resumeId);
        Task<InterviewResponseModel> ContinueInterviewAsync(int resumeId, string userAnswer );        
    }
}
