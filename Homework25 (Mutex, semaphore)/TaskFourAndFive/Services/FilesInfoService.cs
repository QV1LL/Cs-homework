namespace TaskFourAndFive.Services;

internal class FilesInfoService
{
    private readonly List<string> _filePaths = [];

    public void AddFile(string relativeFilePath)
    {
        if (File.Exists(relativeFilePath))
            _filePaths.Add(relativeFilePath);
    }

    public void SaveFilesInfo(string relativeSaveFilePath)
    {
        var reportLines = new List<string>();

        foreach (var filePath in _filePaths)
            reportLines.Add(GetFileInfo(filePath));

        File.WriteAllLines(relativeSaveFilePath, reportLines);
    }

    private static string GetFileInfo(string relativeFilePath)
    {
        var fullPath = Path.GetFullPath(relativeFilePath);
        var fileBytes = File.ReadAllBytes(fullPath);
        var numbers = File.ReadAllLines(fullPath);
        int count = numbers.Length;
        long size = fileBytes.Length;

        var content = string.Join(", ", numbers);

        return $"File: {relativeFilePath}\nCount of numbers: {count}\nSize (bytes): {size}\nContent: {content}\n";
    }
}
