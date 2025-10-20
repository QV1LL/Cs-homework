namespace MicroBrowser.Services.SearchSystems;

internal class DuckDuckGoSearchSystem : ISearchSystem
{
    public string Name => "DuckDuckGo";
    public bool IsEnabled { get; set; } = true;
    public string GetSearchUrl(string query) =>
        $"https://duckduckgo.com/?q={Uri.EscapeDataString(query)}";
}