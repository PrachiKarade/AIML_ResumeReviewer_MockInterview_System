using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Domain.Models;
using ResumeReviewer_WebApiCore.Infrastructure.Repositories;


namespace ResumeReviewer_WebApiCore.Application.Services
{

    #region What this Service Does
    /*
    Pulls resume    
    Gets relevant chunks via RAG    
    Builds GPT prompt
    Calls OpenAI
     */
    #endregion

    public class InterviewService : IInterviewService
    {
        private readonly IResumeRepository _resumeRepository;
        private readonly IRagRepository _ragRepository;
        private readonly IChatService _chatService;
        private readonly ISystemPromptRepository _systemPromptRepository;

        private static readonly Dictionary<int, List<InterviewMessageModel>> _chatHistory = new();

        public InterviewService(IResumeRepository resumeRepository,
                                IRagRepository ragRepository,
                                IChatService chatService,
                                ISystemPromptRepository systemPromptRepository)
        {
            _resumeRepository = resumeRepository;
            _ragRepository = ragRepository;
            _chatService = chatService;
            _systemPromptRepository = systemPromptRepository;
        }

        public async Task<InterviewResponseModel> StartInterviewAsync(int resumeId)
        {
            try
            {
                // 1. Get resume data (experience already extracted earlier)
                var resume = await _resumeRepository.GetByIdAsync(resumeId);

                if (resume == null)
                    throw new Exception("Resume not found");

                // 2. Get best system prompt from master table (for now simple match)
                var systemPrompt = await _systemPromptRepository.GetPromptByExperienceAsync(resume.YearsOfExperience);

                if (systemPrompt == null)
                    throw new Exception("System prompt not configured");

                // 3. Build chat messages
                var messages = new List<InterviewMessageModel>
                {
                    new InterviewMessageModel( InterviewRole.System,
                                              $"{systemPrompt.Content} Ask the first interview question." )
                };

                _chatHistory[resumeId] = messages;
                // 4. Call OpenAI
                var question = await _chatService.CompleteAsync(messages);

                messages.Add(new InterviewMessageModel(InterviewRole.Assistant, question));

                return new InterviewResponseModel { ResumeId = resumeId,InterviewQuestion = question };
            }
            catch (Exception ex)
            {
                // Log exception (not implemented here)
                throw;
            }
        }

        // STEP 2: Continue Interview
        public async Task<InterviewResponseModel> ContinueInterviewAsync(int resumeId, string userAnswer)
        {
            try
            {                              
               var resume = await _resumeRepository.GetByIdAsync(resumeId);

               if (resume == null)
                 throw new Exception("Resume not found");

                if (!_chatHistory.ContainsKey(resumeId))
                    throw new Exception("Interview not started");

                var messages = _chatHistory[resumeId];

                // 1️ Add user answer
                messages.Add(new InterviewMessageModel(InterviewRole.User, userAnswer));

                // 2️ Ask next question
                var nextQuestion = await _chatService.CompleteAsync(messages);

                messages.Add(new InterviewMessageModel(InterviewRole.Assistant,nextQuestion));

                return new InterviewResponseModel
                {
                    ResumeId = resumeId,
                    InterviewQuestion = nextQuestion
                };
            }
            catch (Exception ex)
            {
              throw;
            }
        }
    }
}

