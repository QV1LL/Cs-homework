namespace WordFinderInDirectory.Domain;

internal record WordSearchResult
(
    string FileName,
    string FilePath,
    int Count
);