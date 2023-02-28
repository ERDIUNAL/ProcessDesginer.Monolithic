using Crea.Core.CrossCuttingConcerns.Logging.SeriLog.ConfigurationModals;
using Crea.Core.CrossCuttingConcerns.Logging.SeriLog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Crea.Core.CrossCuttingConcerns.Logging.SeriLog.Loggers;

public class MsSqlLogger : LoggerServiceBase
{
    public MsSqlLogger(IConfiguration configuration)
    {
        var logConfiguration = configuration.GetSection("SeriLogConfigurations:MsSqlConfiguration")
            .Get<MsSqlConfiguration>() ??
            throw new Exception(SeriLogMessages.NullOptionsMessage);

        var sinkOptions = new MSSqlServerSinkOptions()
        {
            TableName = logConfiguration.TableName,
            AutoCreateSqlTable = logConfiguration.AutoCreateSqlTable
        };

        ColumnOptions columnOptions = new();

        var serilogConfig = new LoggerConfiguration()
            .WriteTo.MSSqlServer(
            connectionString: logConfiguration.ConnectionString,
            sinkOptions: sinkOptions,
            columnOptions: columnOptions)
            .CreateLogger();

        Logger = serilogConfig;
    }
}
