using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Web;

namespace MicroImageBoard.Services.SearchSystems;

internal class GoogleCustomSearchImageSystem : IImageSearchSystem
{
    public string Name => "Google";
    public bool IsEnabled { get; set; } = true;

    private readonly string _apiKey;
    private readonly string _searchEngineId;
    private const string Endpoint = "https://www.googleapis.com/customsearch/v1";
    private static readonly HttpClient _client = new();

    public GoogleCustomSearchImageSystem(string apiKey, string searchEngineId)
    {
        _apiKey = apiKey;
        _searchEngineId = searchEngineId;
    }

    public async Task<IEnumerable<string>> GetImageUrlsAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return [];

        try
        {
            string encodedQuery = HttpUtility.UrlEncode(query);
            string url = $"{Endpoint}?key={_apiKey}&cx={_searchEngineId}&q={encodedQuery}&num=10&searchType=image";

            using HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            JObject results = JObject.Parse(json);

            var urls = new List<string>();

            var items = results["items"] as JArray ?? [];
            foreach (var item in items)
            {
                var link = item["link"]?.ToString();
                if (!string.IsNullOrEmpty(link))
                    urls.Add(link);
            }

            return urls;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Search error: {ex.Message}");
            return [];
        }
    }

}
