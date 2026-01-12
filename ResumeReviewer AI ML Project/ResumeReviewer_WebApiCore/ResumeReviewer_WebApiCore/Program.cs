using Microsoft.EntityFrameworkCore;
using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Application.Mappings;
using ResumeReviewer_WebApiCore.Application.Services;
using ResumeReviewer_WebApiCore.Infrastructure.Data;
using ResumeReviewer_WebApiCore.Infrastructure.Embeddings;
using ResumeReviewer_WebApiCore.Infrastructure.Repositories;
using ResumeReviewer_WebApiCore.Infrastructure.ResumeParsing;
using Microsoft.OpenApi.Models;
using ResumeReviewer_WebApiCore.Infrastructure.OpenAI;


var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Automapper
builder.Services.AddAutoMapper(typeof(ResumeMappingProfile));

// Repositories
builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
builder.Services.AddScoped<IRagRepository, RagRepository>();
builder.Services.AddScoped<ISystemPromptRepository, SystemPromptRepository>();
builder.Services.AddScoped<IResumeAnalysisPromptRepository, ResumeAnalysisPromptRepository>();

// Services
builder.Services.AddScoped<IResumeService, ResumeService>();
builder.Services.AddScoped<IRagService, RagService>();
builder.Services.AddScoped<IResumeParser, ResumeParser>();
builder.Services.AddScoped<IInterviewService, InterviewService>();

builder.Services.AddScoped<IChatService, OpenAIChatService>(); //OpenAI Chat service
builder.Services.AddHttpClient<IEmbeddingService, LlamaEmbeddingService>(); // Llama Embedding service

// DbContext (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5178") // React dev server
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

// Middleware pipeline
app.UseHttpsRedirection();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
