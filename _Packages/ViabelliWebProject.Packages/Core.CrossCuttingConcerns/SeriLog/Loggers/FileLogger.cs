using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog.ConfigurationModel;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog.Messages;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog.Loggers;

public class FileLogger : LoggerServiceBase
{
    private readonly IConfiguration configuration;

    public FileLogger(IConfiguration configuration)
    {
        this.configuration = configuration;
        FileConfiguration logconfig = configuration.GetSection("SeriLogConfigurations:FileLogConfiguration").Get<FileConfiguration>() ?? throw new Exception(SeriLogMessages.NullOptionsMessage);

        string logFilePath = string.Format(format: "{0}{1}", arg0: Directory.GetCurrentDirectory() + logconfig.FolderPath, arg1: ".txt");

        Logger = new LoggerConfiguration().WriteTo.File(
            logFilePath,
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: null,
            fileSizeLimitBytes: 5000000,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine} {Exception}").CreateLogger();
    }
}
