using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Domain.Entities;
using ResumeReviewer_WebApiCore.Domain.Models;
using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using ResumeReviewer_WebApiCore.Infrastructure.Repositories;
using ResumeReviewer_WebApiCore.Infrastructure.Data;

namespace ResumeReviewer_WebApiCore.Application.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IResumeParser _parser;
        private readonly IEmbeddingService _embeddingService;
        private readonly IRagService _ragService;

        private readonly IResumeRepository _resumeRepository;
        private readonly IRagRepository _ragRepository;
        private readonly IResumeAnalysisPromptRepository _resumeAnaysisPromptRepository;

        private readonly IMapper _mapper;
        

        public ResumeService(IResumeParser parser,
                             IResumeRepository resumeRepository,
                             IRagRepository ragRepository,
                             IRagService ragService, 
                             IEmbeddingService embeddingService,
                             IResumeAnalysisPromptRepository resumeAnalysisPromptRepository,
                             IMapper mapper)
        {
            _parser = parser;            

            _resumeRepository = resumeRepository;
            _ragRepository = ragRepository;
            _resumeAnaysisPromptRepository = resumeAnalysisPromptRepository;

            _embeddingService = embeddingService;
            _ragService = ragService;
            _mapper = mapper;
        }

        public async Task<int> UploadResumeAsync(IFormFile file)
        {
            try
            {
                // 1️⃣ Parse resume text
                var rawText = await _parser.ParseAsync(file);

                int yearsOfExperience = InferYearsOfExperience(rawText);

                // 2️⃣ Create Entity
                var resumeEntity = new ResumeEntity
                {
                    FileName = file.FileName,
                    RawText = rawText,
                    CreatedAt = DateTime.UtcNow,
                    YearsOfExperience = yearsOfExperience
                };

                // 3️⃣ Save to DB
                var resumeId = await _resumeRepository.SaveAsync(resumeEntity);

                // 4️⃣ Send chunks to RAG for embedding
                await _ragService.ProcessResumeChunksAsync(resumeId, rawText);

                return resumeId;
            }
            catch (Exception ex)
            {
                // Log exception (not implemented here)
                throw new Exception("Error uploading resume", ex);
            }
        }

        public async Task<ResumeModel> GetResumeByIdAsync(int resumeId)
        {
            var entity = await _resumeRepository.GetByIdAsync(resumeId);
            return _mapper.Map<ResumeModel>(entity);
        }

        public async Task<ResumeAnalysisModel> AnalyzeResumeAsync(int resumeId)
        {
            try
            {
                var resumeEntity = await _resumeRepository.GetByIdAsync(resumeId);

                if (resumeEntity == null)
                    throw new Exception($"Resume with ID {resumeId} not found.");

                var model = _mapper.Map<ResumeModel>(resumeEntity);

                var analysis = new ResumeAnalysisModel
                {
                    ResumeId = resumeId,
                    ExperienceLevel = GetExperienceLevel(model.YearsOfExperience),
                    Score = CalculateScore(model),
                    Strengths = new List<string>(),
                    Improvements = new List<string>()
                };

                // 1️ Get all resume chunks
                var chunks = await _ragRepository.GetChunksByResumeIdAsync(resumeId); 

                var chunkEmbeddings = await _ragRepository.GetEmbeddingsByChunksAsync(chunks.Select(c => c.ChunkId).ToList());

                var resumeAnalysisPrompts = await _resumeAnaysisPromptRepository.GetAllAsync();

                if (resumeAnalysisPrompts == null || !resumeAnalysisPrompts.Any())
                {
                    await ResumeAnalysisPromptSeeder.InsertResumeAnalysisPrompt(_resumeAnaysisPromptRepository, _embeddingService);

                    resumeAnalysisPrompts = await _resumeAnaysisPromptRepository.GetAllAsync();
                }

                // 3️ Compare each chunk embedding with prompt embeddings
                foreach (var chunk in chunks)
                {
                    var embeddingEntity = chunkEmbeddings.FirstOrDefault(e => e.ChunkId == chunk.ChunkId);

                    if (embeddingEntity == null) continue;

                    var chunkVector = CosineSimilarityService.BytesToFloatArray(embeddingEntity.Vector);

                    // Strength
                    foreach (var prompt in resumeAnalysisPrompts)
                    {
                        if (prompt.Embedding == null || prompt.Embedding.Length == 0) continue;

                        var promptVector = CosineSimilarityService.BytesToFloatArray(prompt.Embedding);

                        var score = CosineSimilarityService.CosineSimilarity(chunkVector, promptVector);

                        if (score > 0.5f) // threshold for match
                        {
                            switch (prompt.Category)
                            {
                                case "Strength":
                                    analysis.Strengths.Add(prompt.Content);
                                    break;

                                case "Weakness":
                                    analysis.Improvements.Add(prompt.Content);
                                    break;

                                case "Score":
                                    analysis.Score += 5; // increment score for matching score prompts
                                    break;
                            }
                        }
                    }
                }
                // Limit duplicates
                analysis.Strengths = analysis.Strengths.Distinct().ToList();
                analysis.Improvements = analysis.Improvements.Distinct().ToList();
                analysis.Score = Math.Min(analysis.Score, 100);

                return analysis;
            }
            catch (Exception ex)
            {
              throw new Exception("Error analyzing resume", ex);
            }
        }

        #region Helper Methods
        private int InferYearsOfExperience(string text)
        {
            if (text.Contains("12 years", StringComparison.OrdinalIgnoreCase))
                return 12;
            if (text.Contains("10+ years", StringComparison.OrdinalIgnoreCase))
                return 10;
            if (text.Contains("5+ years", StringComparison.OrdinalIgnoreCase))
                return 5;
            if (text.Contains("3+ years", StringComparison.OrdinalIgnoreCase))
                return 3;

            return 1; // default
        }

        private string GetExperienceLevel(int years)
        {
            if(years <= 1) return "Fresher";

            if(years <= 3) return "Junior";

            if (years <= 5) return "Mid-level";

            return "Senior";          
        }

        private int CalculateScore(ResumeModel resume)
        {
            int score = 50; // base score

            // 1️ Reward longer resumes

            if (!string.IsNullOrWhiteSpace(resume.RawText) && resume.RawText.Length > 1500)
                score += 20;

            // 2️ Reward experience

            if (resume.YearsOfExperience >= 3)
                score += 15;
            else if (resume.YearsOfExperience >= 1)
                score += 5;

            // 3️ Reward keyword presence

            string[] keywords = { "project", "achievement", "lead", "managed", "developed" };

            foreach (var keyword in keywords)
            {
                if (resume.RawText.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    score += 5;
            }

            // 4️ Cap at 100
            return Math.Min(score, 100);
        }
                
        #endregion
    }
}
