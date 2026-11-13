
using System;
using System.ServiceProcess;

namespace DatabaseBackupService
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                using (DatabaseBackupService service =new DatabaseBackupService())
                {
                    Console.WriteLine("Database Backup Service - Debug Mode");

                    service.StartDebugging(args);

                    Console.WriteLine("Service is running...");

                    Console.WriteLine("Press ENTER to stop the service.");

                    Console.ReadLine();

                    service.StopDebugging();

                    Console.WriteLine("Service stopped.");

                }
            }
            else
            {
                ServiceBase.Run(
                    new ServiceBase[]
                    {
                        new DatabaseBackupService()
                    });
            }
        }
    }
}
