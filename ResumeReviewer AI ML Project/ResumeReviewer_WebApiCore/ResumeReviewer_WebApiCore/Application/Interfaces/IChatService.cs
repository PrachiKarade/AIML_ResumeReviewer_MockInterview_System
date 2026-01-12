using ResumeReviewer_WebApiCore.Domain.Models;

namespace ResumeReviewer_WebApiCore.Application.Interfaces
{
    public interface IChatService
    {
      Task<string> CompleteAsync(List<InterviewMessageModel> messages);
    }
}
