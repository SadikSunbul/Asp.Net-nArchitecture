using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog.ConfigurationModel;

public class MsSqlConfiguration
{
    public string ConnectonString { get; set; }
    public string TableName { get; set; }
    public bool AutoCreateSqlTable { get; set; }
    public MsSqlConfiguration()
    {
        ConnectonString = string.Empty;
        TableName = string.Empty;
        AutoCreateSqlTable = true;
    }
    public MsSqlConfiguration(string connectionString, string tableName, bool autoCreateSqlTable)
    {
        ConnectonString = connectionString;
        TableName = tableName;
        AutoCreateSqlTable = autoCreateSqlTable;
    }
}
