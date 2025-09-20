using System.IO;
using System.Text.RegularExpressions;
using WordFinderInDirectory.Domain;

namespace WordFinderInDirectory.Services;

internal static class DirectoryWordFinder
{
    public static async Task SearchAsync(string word, string directory, IProgress<WordSearchResult> progress)
    {
        var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

        var tasks = files.Select(async file =>
        {
            int count = 0;
            try
            {
                string text = await File.ReadAllTextAsync(file);
                count = text.Split([word], StringSplitOptions.None).Length - 1;
            }
            catch
            {
                // ...
            }

            progress.Report(new WordSearchResult
            (
                Path.GetFileName(file),
                file,
                count
            ));
        });

        await Task.WhenAll(tasks);
    }
}
