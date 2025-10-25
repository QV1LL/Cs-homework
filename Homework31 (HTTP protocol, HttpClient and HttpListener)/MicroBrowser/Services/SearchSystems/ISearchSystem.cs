using SearchResult = System.Collections.Generic.Dictionary<string, string>;

namespace MicroBrowser.Services.SearchSystems;

internal interface ISearchSystem
{
    public string Name { get; }
    public bool IsEnabled { get; set; }

    Task<SearchResult> GetSearchResultsAsync(string query);
}
