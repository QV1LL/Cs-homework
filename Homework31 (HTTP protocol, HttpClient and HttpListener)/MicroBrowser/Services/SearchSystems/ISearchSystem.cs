namespace MicroBrowser.Services.SearchSystems;

internal interface ISearchSystem
{
    public string Name { get; }
    public bool IsEnabled { get; set; }

    string GetSearchUrl(string query);
}
