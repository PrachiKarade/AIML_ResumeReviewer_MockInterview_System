using ResumeReviewer_WebApiCore.Application.Interfaces;
using System.Net.Http.Json;

namespace ResumeReviewer_WebApiCore.Infrastructure.Embeddings
{
    public class LlamaEmbeddingService :IEmbeddingService
    {
        private readonly HttpClient _httpClient;

        public LlamaEmbeddingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public float[] GenerateEmbedding(string text)
        {
            try
            {
                // TODO: Connect to local LLaMA model to generate embeddings

                var request = new { model = "nomic-embed-text", prompt = text };// nomic-embed-text is an example model name

                var response = _httpClient
                               .PostAsJsonAsync("http://localhost:11434/api/embeddings", request)
                               .Result;


                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>().Result;

                return result!.Embedding;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., logging)
                throw new Exception("Error generating embedding", ex);
            }
        }

        private class OllamaEmbeddingResponse
        {
            public float[] Embedding { get; set; } = Array.Empty<float>();
        }
    }  

}
