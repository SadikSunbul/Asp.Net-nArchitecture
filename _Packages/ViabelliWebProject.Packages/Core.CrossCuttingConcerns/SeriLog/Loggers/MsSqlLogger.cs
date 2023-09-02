using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog.ConfigurationModel;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog.Messages;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog.Loggers;

public class MsSqlLogger : LoggerServiceBase
{
    public MsSqlLogger(IConfiguration configuration)
    {
        MsSqlConfiguration logConfiguration = configuration.GetSection("SeriLogConfigurations:MsSqlConfigurationSadıkDocker").Get<MsSqlConfiguration>() ?? throw new Exception(SeriLogMessages.NullOptionsMessage);

        MSSqlServerSinkOptions sinkOptions = new()
        {
            TableName = logConfiguration.TableName,
            AutoCreateSqlDatabase = logConfiguration.AutoCreateSqlTable
        };

        ColumnOptions columnOptions = new();

        Logger serilogConfig = new LoggerConfiguration().WriteTo.MSSqlServer(logConfiguration.ConnectonString, sinkOptions, columnOptions: columnOptions).CreateLogger();

        Logger = serilogConfig;
    }
}

/*
 Tablo oluşmaz ise veri tabanı kısmında bu kodu verı tabanınızda calsıtıra bilirsiniz

CREATE TABLE [Logs] (

   [Id] int IDENTITY(1,1) NOT NULL,
   [Message] nvarchar(max) NULL,
   [MessageTemplate] nvarchar(max) NULL,
   [Level] nvarchar(128) NULL,
   [TimeStamp] datetime NOT NULL,
   [Exception] nvarchar(max) NULL,
   [Properties] nvarchar(max) NULL

   CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([Id] ASC)
);
 */