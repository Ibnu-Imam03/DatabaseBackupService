
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace DatabaseBackupService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public ProjectInstaller()
        {
            // Configure service account
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account =ServiceAccount.LocalSystem;

            // Configure Windows Service
            serviceInstaller = new ServiceInstaller();

            serviceInstaller.ServiceName ="DatabaseBackupService";

            serviceInstaller.DisplayName ="Database Backup Service";

            serviceInstaller.Description ="Automatically performs periodic SQL Server database backups.";

            serviceInstaller.StartType =ServiceStartMode.Automatic;

            // Configure service dependencies
            serviceInstaller.ServicesDependedOn =
                new string[]
                {
                    "MSSQLSERVER",
                    "RpcSs",
                    "EventLog"
                };

            // Register installers
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
