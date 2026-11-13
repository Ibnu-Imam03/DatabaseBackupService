
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace DatabaseBackupService
{
    public static class DatabaseBackup
    {
        public static void PerformBackup()
        {
            try
            {
                // Read configuration
                string connectionString =ConfigurationManager.AppSettings["ConnectionString"];

                string backupFolder =ConfigurationManager.AppSettings["BackupFolder"];

                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new ConfigurationErrorsException("ConnectionString is missing.");

                if (string.IsNullOrWhiteSpace(backupFolder))
                    throw new ConfigurationErrorsException("BackupFolder is missing.");

                // Extract database name
                SqlConnectionStringBuilder builder =new SqlConnectionStringBuilder(connectionString);

                string databaseName = builder.InitialCatalog;

                if (string.IsNullOrWhiteSpace(databaseName))
                    throw new ConfigurationErrorsException("Database name is missing from ConnectionString.");

                // Create backup directory
                Directory.CreateDirectory(backupFolder);

                // Generate timestamped filename
                string safeDatabaseName = databaseName;

                foreach (char c in Path.GetInvalidFileNameChars())  safeDatabaseName = safeDatabaseName.Replace(c, '_');

                string fileName =$"Backup_{safeDatabaseName}_{DateTime.Now:yyyyMMdd_HHmmss_fff}.bak";

                string backupPath =Path.Combine(backupFolder, fileName);

                // Safely construct SQL identifiers and file path
                string sqlDatabaseName ="[" + databaseName.Replace("]", "]]") + "]";

                string sqlBackupPath =backupPath.Replace("'", "''");

                string query =
                    $"BACKUP DATABASE {sqlDatabaseName} " +
                    $"TO DISK = N'{sqlBackupPath}' " +
                    "WITH INIT, CHECKSUM, STATS = 10;";

                // Connect and execute backup
                using (SqlConnection connection =new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command =new SqlCommand(query, connection))
                    {
                        // Allow large databases to finish backing up
                        command.CommandTimeout = 0;

                        command.ExecuteNonQuery();
                    }
                }

                Logger.Log($"Database backup successful: {backupPath}");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error during database backup", ex);

                throw;
            }
        }
    }
}
