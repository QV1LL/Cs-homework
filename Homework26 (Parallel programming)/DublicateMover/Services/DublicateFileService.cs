using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DublicateMover.Services;

internal class DuplicateFileService : IDuplicateFileService
{
    public async Task<string> ProcessDirectoryAsync(string sourceDir, string destDir, Action<string> logAction, CancellationToken token)
    {
        var report = new StringBuilder();
        var files = Directory.GetFiles(sourceDir, ".", SearchOption.AllDirectories);
        var hashGroups = new Dictionary<string, List<string>>();
        foreach (var file in files)
        {
            token.ThrowIfCancellationRequested();
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    var hash = BitConverter.ToString(await md5.ComputeHashAsync(stream)).Replace("-", "").ToLowerInvariant();
                    if (!hashGroups.ContainsKey(hash))
                    {
                        hashGroups[hash] = new List<string>();
                    }
                    hashGroups[hash].Add(file);
                }
            }
            var processLine = $"Оброблено файл: {file}";
            report.AppendLine(processLine);
            logAction?.Invoke(processLine);
        }
        foreach (var group in hashGroups)
        {
            token.ThrowIfCancellationRequested();
            var filesInGroup = group.Value;
            if (filesInGroup.Count > 0)
            {
                var original = filesInGroup[0];
                var destFileName = Path.GetFileName(original);
                var destPath = Path.Combine(destDir, destFileName);
                int counter = 1;
                string baseName = Path.GetFileNameWithoutExtension(original);
                string extension = Path.GetExtension(original);
                while (File.Exists(destPath))
                {
                    destFileName = $"{baseName} ({counter}){extension}";
                    destPath = Path.Combine(destDir, destFileName);
                    counter++;
                }
                File.Move(original, destPath);
                var moveLine = $"Переміщено оригінал: {original} до {destPath}";
                report.AppendLine(moveLine);
                logAction?.Invoke(moveLine);
            }
        }
        return report.ToString();
    }
}
