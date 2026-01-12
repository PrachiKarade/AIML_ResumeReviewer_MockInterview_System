using OpenAI;
using OpenAI.Chat;
using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Domain.Models;


namespace ResumeReviewer_WebApiCore.Infrastructure.OpenAI
{
    public class OpenAIChatService : IChatService
    {
        private readonly ChatClient _chatClient;
        public OpenAIChatService()
        {
            var apiKey = Environment.GetEnvironmentVariable("OpenAiKey");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "OpenAI API key is not set. Please configure environment variable 'OpenAiKey'.");
            }

            var openAiClient = new OpenAIClient(apiKey);
            _chatClient = openAiClient.GetChatClient("gpt-4o-mini");
        }

        public async Task<string> CompleteAsync(List<InterviewMessageModel> messages)
        {
            try
            {     
                            
                if (messages == null || !messages.Any())
                    throw new ArgumentException("Messages cannot be empty");

                // Convert domain messages → OpenAI messages
                var chatMessages = new List<ChatMessage>();

                foreach (var message in messages)
                {
                    switch (message.Role)
                    {
                        case InterviewRole.System:
                            chatMessages.Add(new SystemChatMessage(message.Content));
                            break;

                        case InterviewRole.User:
                            chatMessages.Add(new UserChatMessage(message.Content));
                            break;

                        case InterviewRole.Assistant:
                            chatMessages.Add(new AssistantChatMessage(message.Content));
                            break;
                    }
                }

                var completion = await _chatClient.CompleteChatAsync(chatMessages);

                return completion.Value.Content.Last().Text;
            }    
            catch (Exception ex)
            {
                // Log exception (not implemented here)
                throw;
            }
        }        
    }
}
