using Microsoft.EntityFrameworkCore;
using PromptGenerator.Models;
using System.IO;

namespace PromptGenerator.Data
{
    public class PromptDbContext : DbContext
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        public DbSet<ApplicationDomain> ApplicationDomains { get; set; }
        public DbSet<Step> Steps { get; set; }

        private readonly string _dbPath;

        public PromptDbContext()
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "data");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            _dbPath = Path.Combine(folder, "promptgenerator.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
