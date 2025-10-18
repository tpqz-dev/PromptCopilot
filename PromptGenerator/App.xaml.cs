using System;
using System.Windows;
using PromptGenerator.Services;
using PromptGenerator.Data;

namespace PromptGenerator
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppLogger.Init();

            try
            {
                using var db = new PromptDbContext();
                db.Database.EnsureCreated();
                AppLogger.Log.Information("Database ensured/created");
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error(ex, "Database initialization error");
            }
        }
    }
}
