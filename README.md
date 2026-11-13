# Database Backup Windows Service

A Windows Service developed using **C# and .NET Framework 4.8** that automatically creates full backups of a SQL Server database at configurable time intervals.

## 1. Project Overview

DatabaseBackupService automates SQL Server database backups without requiring manual execution.

The service connects to SQL Server, executes a full database backup, and saves the resulting `.bak` file with a timestamp. It also maintains daily log files to record service activities, successful backups, and errors.

## 2. Features

- Automatic SQL Server database backups.
- Full database backup using T-SQL `BACKUP DATABASE`.
- Timestamped `.bak` backup files.
- Configurable backup interval.
- Configurable database connection string.
- Configurable backup and log directories.
- Daily logging of service activities and errors.
- Console mode for debugging and manual testing.
- Windows Service installation and automatic startup.
- SQL Server service dependency configuration.

## 3. Technologies Used

- C#
- .NET Framework 4.8
- Windows Services
- ADO.NET
- SQL Server
- T-SQL
- `System.Timers`
- `App.config`

## 4. Project Structure

| File | Responsibility |
|---|---|
| `Program.cs` | Application entry point and console/service execution |
| `DatabaseBackupService.cs` | Windows Service lifecycle and backup timer |
| `DatabaseBackup.cs` | SQL Server connection and backup execution |
| `Logger.cs` | Daily logging and error recording |
| `ProjectInstaller.cs` | Windows Service installation configuration |
| `App.config` | Application settings |

## 5. Configuration

The service uses `App.config` to manage its settings.

Example:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>

    <add key="ConnectionString"
         value="Server=.;Database=C21_DB1;Trusted_Connection=True;" />

    <add key="BackupFolder"
         value="C:\DatabaseBackups" />

    <add key="LogFolder"
         value="C:\DatabaseBackups\Logs" />

    <add key="BackupIntervalMinutes"
         value="60" />

  </appSettings>
</configuration>
```

### Configuration settings

| Setting | Description |
|---|---|
| `ConnectionString` | SQL Server connection information |
| `BackupFolder` | Directory where backup files are stored |
| `LogFolder` | Directory where log files are stored |
| `BackupIntervalMinutes` | Time between consecutive backups |

**Important:** SQL Server's Database Engine service account must have permission to write to the backup directory. The Windows Service account must also have appropriate SQL Server database permissions.

## 6. Backup Process

The service follows this workflow:

1. Starts when Windows launches the service or an administrator starts it.
2. Reads configuration settings.
3. Initializes the backup timer.
4. Connects to the configured SQL Server database.
5. Executes a full database backup.
6. Generates a timestamped `.bak` file.
7. Records the backup result in the daily log.
8. Schedules the next backup according to the configured interval.

### Example backup filename

```text
Backup_C21_DB1_20261113_151355_841.bak
```

## 7. Logging

The service generates daily log files inside the configured log directory.

Example:

```text
C:\DatabaseBackups\Logs\
    BackupService_20261113.log
```

Example log entries:

```text
[2026-11-13 15:13:55] Service Started.

[2026-11-13 15:14:03] Database backup successful:
C:\DatabaseBackups\Backup_C21_DB1_20261113_151355_841.bak
```

The logger records:
- Service startup.
- Service shutdown.
- Successful database backups.
- Database connection and backup errors.

## 8. Building the Project

Requirements:

- Windows operating system.
- Visual Studio.
- .NET Framework 4.8.
- SQL Server.
- Appropriate SQL Server permissions.

Steps:

1. Clone or download the repository.
2. Open `DatabaseBackupService.sln` in Visual Studio (or the solution file provided).
3. Configure the connection string and backup settings in `App.config`.
4. Select the `Release` configuration.
5. Build the solution.
6. Locate the executable in:

```text
bin\Release\DatabaseBackupService.exe
```

The generated configuration file is:

```text
bin\Release\DatabaseBackupService.exe.config
```

## 9. Installation and Deployment

Run Command Prompt as Administrator.

### Install the service

For a 64-bit Windows installation:

```cmd
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe "C:\Path\To\DatabaseBackupService.exe"
```

Replace the executable path with the actual Release executable location.

### Start the service

```cmd
sc start DatabaseBackupService
```

### Check service status

```cmd
sc query DatabaseBackupService
```

### Stop the service

```cmd
sc stop DatabaseBackupService
```

### Uninstall the service

```cmd
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe /u "C:\Path\To\DatabaseBackupService.exe"
```

## 10. Console Debugging

The application supports interactive console execution using `Environment.UserInteractive`.

This allows the application to be tested without installing or running it through Windows Service Control Manager.

In console mode:

1. The application starts.
2. The backup process executes.
3. Log messages are displayed in the console.
4. Pressing Enter stops the debugging session.

This makes development and troubleshooting easier.

## 11. Testing and Verification

The following tests were performed during development.

| Test | Result |
|---|---|
| Console debugging | Passed |
| Windows Service installation | Passed |
| Automatic startup configuration | Verified |
| SQL Server connection | Passed |
| Database backup execution | Passed |
| Timestamped backup file creation | Passed |
| Backup integrity verification | Passed |
| Periodic backup execution | Tested |

### Backup integrity verification

The backup was verified using SQL Server:

```sql
RESTORE VERIFYONLY
FROM DISK = N'C:\DatabaseBackups\Backup_C21_DB1_20261113_151355_841.bak'
WITH CHECKSUM;
```

SQL Server returned:

```text
The backup set on file 1 is valid.
```

**Additional tests not performed:**
- Invalid connection string and failure handling.
- SQL Server dependency startup test.

## 12. Project Purpose and Learning Outcomes

This project demonstrates practical experience with:

- Windows Service development.
- Service lifecycle management.
- ADO.NET database connectivity.
- SQL Server backup operations.
- Timer-based task scheduling.
- Configuration management.
- File and directory management.
- Exception handling and logging.
- Windows Service installation and deployment.

## 13. Author

**Abubakar Yimam**

Software Engineering Student

GitHub: [Ibnu-Imam03](https://github.com/Ibnu-Imam03)

---

*DatabaseBackupService — Automated SQL Server Database Backup using C# and Windows Services.*
