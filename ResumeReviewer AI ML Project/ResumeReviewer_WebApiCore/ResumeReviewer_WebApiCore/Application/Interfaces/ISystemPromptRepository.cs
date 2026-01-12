using ResumeReviewer_WebApiCore.Domain.Entities;
using ResumeReviewer_WebApiCore.Domain.Models;

namespace ResumeReviewer_WebApiCore.Application.Interfaces
{
    public interface ISystemPromptRepository
    {
        Task<SystemPromptEntity?> GetPromptByExperienceAsync(int yearsOfexperience);
        //Task StoreSystemPromptAsync(SystemPromptEntity prompt);
    }
}
