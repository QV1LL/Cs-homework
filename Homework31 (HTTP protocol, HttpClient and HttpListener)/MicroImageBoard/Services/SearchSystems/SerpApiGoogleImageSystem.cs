using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Web;

namespace MicroImageBoard.Services.SearchSystems;

internal class SerpApiGoogleImageSystem : IImageSearchSystem
{
    public string Name => "SerpApi Google";
    public bool IsEnabled { get; set; } = true;

    private readonly string _apiKey;
    private const string Endpoint = "https://serpapi.com/search";
    private static readonly HttpClient _client = new();

    public SerpApiGoogleImageSystem(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<IEnumerable<string>> GetImageUrlsAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return [];

        try
        {
            string encodedQuery = HttpUtility.UrlEncode(query);
            string url = $"{Endpoint}?engine=google_images&q={encodedQuery}&api_key={_apiKey}&num=10";

            using HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            JObject results = JObject.Parse(json);

            var urls = new List<string>();

            var items = results["images_results"] as JArray ?? [];
            foreach (var item in items)
            {
                var link = item["original"]?.ToString();
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