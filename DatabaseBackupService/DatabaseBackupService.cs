
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace DatabaseBackupService
{
    public partial class DatabaseBackupService : ServiceBase
    {
        private Timer _backupTimer;
        private readonly object _timerLock = new object();
        private bool _isRunning;
        private double _backupInterval;

        public DatabaseBackupService()
        {
            InitializeComponent();

            ServiceName = "DatabaseBackupService";
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // Read backup interval
                string interval =ConfigurationManager.AppSettings["BackupIntervalMinutes"];

                if (!double.TryParse(interval, out double minutes) || minutes <= 0 || minutes * 60000 > int.MaxValue)
                {
                    throw new ConfigurationErrorsException("Invalid BackupIntervalMinutes configuration.");
                }

                _backupInterval = minutes * 60000;

                lock (_timerLock)
                {
                    _isRunning = true;

                    // Create timer
                    _backupTimer = new Timer(1);

                    _backupTimer.AutoReset = false;

                    _backupTimer.Elapsed +=OnBackupTimerElapsed;

                    // First backup runs immediately
                    _backupTimer.Start();
                }

                Logger.Log("Service Started.");
            }
            catch (Exception ex)
            {
                Logger.LogError(
                    "Service startup failed", ex);

                throw;
            }
        }

        private void OnBackupTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!_isRunning)
                    return;

                DatabaseBackup.PerformBackup();
            }
            catch (Exception)
            {
                // DatabaseBackup already logs detailed errors.
            }
            finally
            {
                lock (_timerLock)
                {
                    // Schedule the next backup
                    if (_isRunning && _backupTimer != null)
                    {
                        _backupTimer.Interval = _backupInterval;
                        _backupTimer.Start();
                    }
                }
            }
        }

        protected override void OnStop()
        {
            lock (_timerLock)
            {
                _isRunning = false;

                if (_backupTimer != null)
                {
                    _backupTimer.Stop();
                    _backupTimer.Dispose();
                    _backupTimer = null;
                }
            }

            Logger.Log("Service Stopped.");
        }
    }
}
