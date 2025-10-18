using PromptGenerator.Data;
using PromptGenerator.Models;
using System.Linq;

namespace PromptGenerator.Services
{
    public static class DataSeeder
    {
        public static void SeedDemoData()
        {
            using var db = new PromptDbContext();
            db.Database.EnsureCreated();

            if (!db.Languages.Any())
            {
                db.Languages.AddRange(
                    new Language { Name = "C#" },
                    new Language { Name = "Python" },
                    new Language { Name = "JavaScript" }
                );
                AppLogger.Log.Information("Seeded Languages");
            }

            if (!db.ApplicationTypes.Any())
            {
                db.ApplicationTypes.AddRange(
                    new ApplicationType { Name = "Console" },
                    new ApplicationType { Name = "Web API" },
                    new ApplicationType { Name = "Desktop (WPF)" }
                );
                AppLogger.Log.Information("Seeded ApplicationTypes");
            }

            if (!db.ApplicationDomains.Any())
            {
                db.ApplicationDomains.AddRange(
                    new ApplicationDomain { Name = "Finance" },
                    new ApplicationDomain { Name = "Healthcare" },
                    new ApplicationDomain { Name = "Education" }
                );
                AppLogger.Log.Information("Seeded ApplicationDomains");
            }

            if (!db.Steps.Any())
            {
                db.Steps.AddRange(
                    new Step { Description = "Décrire le but principal de l'application", OrderIndex = 1 },
                    new Step { Description = "Lister les fonctionnalités clés", OrderIndex = 2 },
                    new Step { Description = "Préciser les contraintes non fonctionnelles", OrderIndex = 3 }
                );
                AppLogger.Log.Information("Seeded Steps");
            }

            db.SaveChanges();
        }
    }
}
