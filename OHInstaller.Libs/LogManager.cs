using PCLUntils;
using PCLUntils.Objects;
using PCLUntils.Plantform;
using Serilog;
using Serilog.Core;
using System;
using System.IO;

namespace OHInstaller.Libs
{
    public sealed class LogManager
    {
        private static LogManager instance;
        public static LogManager Instance
        {
            get
            {
                instance ??= new LogManager();
                return instance;
            }
        }
        private readonly string folder;
        public string LogFolder => folder;
        private readonly Logger logger;
        private LogManager()
        {
            folder = PlantformUntils.System == Platforms.Android ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) : Extension.GetRoot(true);
            var logFile = Path.Combine(folder, "ohinstall_log", "log-.txt");
            logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Async(a => a.File(logFile, rollingInterval: RollingInterval.Day)).CreateLogger();
        }
        public void LogError(string messageTemplate, Exception exception)
        {
            try
            {
                if (exception.IsNotEmpty())
                    logger.Error(exception, messageTemplate);
            }
            catch { }
        }
        public void LogInfo(string messageTemplate)
        {
            try
            {
                logger.Information(messageTemplate);
            }
            catch { }
        }
        public void LogInfo(string messageTemplate, params object[] propertyValues)
        {
            try
            {
                logger.Information(messageTemplate, propertyValues);
            }
            catch { }
        }
    }
}