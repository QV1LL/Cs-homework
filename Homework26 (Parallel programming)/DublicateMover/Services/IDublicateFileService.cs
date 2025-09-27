namespace DublicateMover.Services;

internal interface IDuplicateFileService
{
    Task<string> ProcessDirectoryAsync(string sourceDir, string destDir, Action<string> logAction, CancellationToken token);
}
