using System.Data;
using Microsoft.EntityFrameworkCore;
using ResumeReviewer_WebApiCore.Domain.Entities;


namespace ResumeReviewer_WebApiCore.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<ResumeEntity> Resumes { get; set; }
        public DbSet<ResumeChunkEntity> ResumeChunks { get; set; }
        public DbSet<ResumeEmbeddingEntity> ResumeEmbeddings { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResumeEntity>().ToTable("Resumes");
            modelBuilder.Entity<ResumeChunkEntity>().ToTable("ResumeChunks");
            modelBuilder.Entity<ResumeEmbeddingEntity>().ToTable("ResumeEmbeddings");
        }

    }
}
