using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using SearchResult = System.Collections.Generic.Dictionary<string, string>;

namespace MicroBrowser.Services.SearchSystems;

internal class SerpApiGoogleSearchSystem : ISearchSystem
{
    public string Name => "SerpApi Google";
    public bool IsEnabled { get; set; } = true;

    private readonly string _apiKey;
    private const string Endpoint = "https://serpapi.com/search";
    private static readonly HttpClient _client = new();

    public SerpApiGoogleSearchSystem(string apiKey)
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
    }

    public async Task<SearchResult> GetSearchResultsAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return new SearchResult();

        try
        {
            string encodedQuery = HttpUtility.UrlEncode(query);
            string url = $"{Endpoint}?engine=google&q={encodedQuery}&api_key={_apiKey}&num=10";

            using HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            JObject results = JObject.Parse(json);

            var result = new SearchResult();

            var items = results["organic_results"] as JArray ?? new JArray();
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
            return new SearchResult();
        }
    }
}