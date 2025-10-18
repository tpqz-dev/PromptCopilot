using Serilog;
using System;
using System.IO;

namespace PromptGenerator.Services
{
    public static class AppLogger
    {
        private static bool _initialized = false;

        public static void Init()
        {
            if (_initialized) return;

            var logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
            var logFile = Path.Combine(logDir, "log.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logFile, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14, shared: true)
                .CreateLogger();

            _initialized = true;
            Log.Information("Logger initialized");
        }

        public static Serilog.ILogger Log => Serilog.Log.Logger;
    }
}
