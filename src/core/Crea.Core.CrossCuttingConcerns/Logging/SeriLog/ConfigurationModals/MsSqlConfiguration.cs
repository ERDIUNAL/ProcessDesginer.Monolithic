namespace Crea.Core.CrossCuttingConcerns.Logging.SeriLog.ConfigurationModals;

public class MsSqlConfiguration
{
    public string ConnectionString { get; set; }
    public string TableName { get; set; }
    public bool AutoCreateSqlTable { get; set; }
}
