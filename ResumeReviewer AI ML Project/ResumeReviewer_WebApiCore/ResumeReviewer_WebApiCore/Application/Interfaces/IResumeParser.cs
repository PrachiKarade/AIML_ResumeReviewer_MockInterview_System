namespace ResumeReviewer_WebApiCore.Application.Interfaces
{
    public interface IResumeParser
    {
        public Task<string> ParseAsync(IFormFile file);        
    }
}
