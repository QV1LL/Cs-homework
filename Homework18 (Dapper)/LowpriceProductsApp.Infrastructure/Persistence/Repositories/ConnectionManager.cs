using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data.Common;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories;

public class ConnectionManager
{
    public string ConnectionString { get; }
    public readonly string ProviderName = AppConfiguration.ProviderName;
    public static DbProviderFactory DbProviderFactory => SqliteFactory.Instance;

    public ConnectionManager()
    {
        Batteries.Init();
        var dbFile = DatabaseFileService.GetDbFilePath();
        ConnectionString = $"Data Source={dbFile}";
    }

    public DbConnection GetConnection()
    {
        var dbConnection = DbProviderFactory.CreateConnection();
        dbConnection.ConnectionString = ConnectionString;
        return dbConnection;
    }
}
