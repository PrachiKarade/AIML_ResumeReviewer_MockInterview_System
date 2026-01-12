using Microsoft.Identity.Client;
using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Domain.Entities;
using ResumeReviewer_WebApiCore.Infrastructure.Repositories;

namespace ResumeReviewer_WebApiCore.Infrastructure.Data
{
    public class ResumeAnalysisPromptSeeder
    {

        public static async Task InsertResumeAnalysisPrompt(IResumeAnalysisPromptRepository promptRepository, IEmbeddingService embeddingService)
        {

            try
            {
                // Check if any prompts already exist
                var existingPrompts = await promptRepository.GetAllAsync();

                if (existingPrompts.Any()) return; 

                // Strength prompts
                var strengthPrompts = new List<string>
                {
                    "Shows strong project experience and problem-solving skills",
                    "Demonstrates leadership in team projects",
                    "Has solid technical knowledge relevant to the role"
                };

                foreach (var prompt in strengthPrompts)
                {
                    float[] embeddingVector = embeddingService.GenerateEmbedding(prompt);

                    var embeddingBytes = FloatArrayToBytes(embeddingVector);

                    await promptRepository.SaveAsync(new ResumeAnalysisPromptEntity
                    {
                        Category = "Strength",
                        Content = prompt,
                        Embedding = embeddingBytes
                    });
                }

                // Weakness prompts
                var weaknessPrompts = new List<string>
                {
                    "Lacks measurable achievements in previous roles",
                    "Limited leadership or managerial experience",
                    "Few side projects or extracurriculars"
                };

                foreach (var prompt in weaknessPrompts)
                {
                    var embedding = embeddingService.GenerateEmbedding(prompt);

                    var entity = new ResumeAnalysisPromptEntity
                    {
                        Category = "Weakness",
                        Content = prompt,
                        Embedding = FloatArrayToBytes(embedding)
                    };
                    await promptRepository.SaveAsync(entity);
                }

                // Score prompts
                var scorePrompts = new List<string>
                {
                  "Resume demonstrates strong experience and skills",
                  "Resume lacks key achievements or clarity"
                };

                foreach (var prompt in scorePrompts)
                {
                    var embedding = embeddingService.GenerateEmbedding(prompt);
                    var entity = new ResumeAnalysisPromptEntity
                    {
                        Category = "Score",
                        Content = prompt,
                        Embedding = FloatArrayToBytes(embedding)
                    };
                    await promptRepository.SaveAsync(entity);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error seeding resume analysis prompts", ex);
            }
        }

        private static byte[] FloatArrayToBytes(float[] floats)
        {
            var bytes = new byte[floats.Length * 4];
            Buffer.BlockCopy(floats, 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
