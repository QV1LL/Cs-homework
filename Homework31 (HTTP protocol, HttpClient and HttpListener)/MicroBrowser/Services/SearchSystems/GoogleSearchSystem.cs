namespace MicroBrowser.Services.SearchSystems;

internal class GoogleSearchSystem : ISearchSystem
{
    public string Name => "Google";
    public bool IsEnabled { get; set; } = true;
    public string GetSearchUrl(string query) =>
        $"https://www.google.com/search?q={Uri.EscapeDataString(query)}";
}
