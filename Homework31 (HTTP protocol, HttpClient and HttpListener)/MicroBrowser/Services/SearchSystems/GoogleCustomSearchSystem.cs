using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using SearchResult = System.Collections.Generic.Dictionary<string, string>;

namespace MicroBrowser.Services.SearchSystems;

internal class GoogleCustomSearchSystem : ISearchSystem
{
    public string Name => "Google";
    public bool IsEnabled { get; set; } = true;

    private readonly string _apiKey;
    private readonly string _searchEngineId;
    private const string Endpoint = "https://www.googleapis.com/customsearch/v1";
    private static readonly HttpClient _client = new();

    public GoogleCustomSearchSystem(string apiKey, string searchEngineId)
    {
        _apiKey = apiKey;
        _searchEngineId = searchEngineId;
    }

    public async Task<SearchResult> GetSearchResultsAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return [];

        try
        {
            string encodedQuery = HttpUtility.UrlEncode(query);
            string url = $"{Endpoint}?key={_apiKey}&cx={_searchEngineId}&q={encodedQuery}&num=10";

            using HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            JObject results = JObject.Parse(json);

            var result = new SearchResult();

            var items = results["items"] as JArray ?? new JArray();
            foreach (var item in items)
            {
                var title = item["title"]?.ToString();
                var link = item["link"]?.ToString();

                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(link))
                    result[title] = link;
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Search error: {ex.Message}");
            return [];
        }
    }
}