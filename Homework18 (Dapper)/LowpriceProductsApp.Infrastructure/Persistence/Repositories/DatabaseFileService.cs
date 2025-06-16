using System;
using System.Diagnostics;
using System.IO;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories;

internal static class DatabaseFileService
{
    internal static string GetDbFilePath()
    {
        var sourceDbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConfiguration.DbFileName);
        var targetDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), AppConfiguration.AppName, "Data");
        var targetDbPath = Path.Combine(targetDirectory, AppConfiguration.DbFileName);

        try
        {
            if (!File.Exists(targetDbPath))
            {
                Directory.CreateDirectory(targetDirectory);
                File.Copy(sourceDbPath, targetDbPath);
            }
        }
        catch (Exception e) { Debug.WriteLine(e); }

        return targetDbPath;
    }
}
