namespace MicroBrowser.Services.SearchSystems;

internal class BingSearchSystem : ISearchSystem
{
    public string Name => "Bing";
    public bool IsEnabled { get; set; } = true;
    public string GetSearchUrl(string query) =>
        $"https://www.bing.com/search?q={Uri.EscapeDataString(query)}";
}
