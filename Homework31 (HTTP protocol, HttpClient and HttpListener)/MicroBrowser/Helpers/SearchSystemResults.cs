using CommunityToolkit.Mvvm.ComponentModel;
using MicroBrowser.Services.SearchSystems;
using System.Collections.ObjectModel;
using SearchResult = System.Collections.Generic.KeyValuePair<string, string>;

namespace MicroBrowser.Helpers;

internal class SearchSystemResults : ObservableObject
{
    public ISearchSystem System { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public ObservableCollection<SearchResult> Results { get; set; } = new();
}