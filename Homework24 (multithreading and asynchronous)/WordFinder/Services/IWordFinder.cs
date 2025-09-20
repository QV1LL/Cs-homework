namespace WordFinder.Services;

internal interface IWordFinder
{
    Task<int> GetCountOfWordAppearenceAsync(string word, string fileAbsolutePath);
}
