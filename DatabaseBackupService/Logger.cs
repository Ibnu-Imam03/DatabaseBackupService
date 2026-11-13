using System;
using System.Configuration;
using System.IO;

namespace DatabaseBackupService
{
    public static class Logger
    {
        private static readonly object _lock = new object();

        public static void Log(string message)
        {
            try
            {
                string logFolder =ConfigurationManager.AppSettings["LogFolder"];

                if (string.IsNullOrWhiteSpace(logFolder))
                    throw new ConfigurationErrorsException("LogFolder is not configured.");

                Directory.CreateDirectory(logFolder);

                string fileName =$"BackupService_{DateTime.Now:yyyyMMdd}.log";

                string filePath = Path.Combine(logFolder, fileName);

                string logMessage =$"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

                lock (_lock)
                {
                    File.AppendAllText(filePath,logMessage + Environment.NewLine);
                }

                if (Environment.UserInteractive)
                {
                    Console.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                // Fallback if normal logging fails.
                if (Environment.UserInteractive)
                {
                    Console.WriteLine("Logging Error: " + ex.Message);
                }
            }
        }

        public static void LogError(string message, Exception ex)
        {
            Log($"{message}: {ex}");
        }
    }
}