using System.Net.Http.Json;
using System.Text.Json;
using XChat.UI.Shared.Dto.User;

namespace XChat.UI.Shared.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AuthService(IHttpClientFactory httpFactory)
    {
        _http = httpFactory.CreateClient("ApiClient");
    }

    public async Task<AuthResponse?> RegisterAsync(AuthRequest user)
    {
        var request = new AuthRequest(user.Name, user.Password);
        var response = await _http.PostAsJsonAsync("api/auth/register", request);

        if (!response.IsSuccessStatusCode)
            return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        try
        {
            return JsonSerializer.Deserialize<AuthResponse>(responseBody, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<AuthResponse?> LoginAsync(AuthRequest user)
    {
        var request = new AuthRequest(user.Name, user.Password);
        var response = await _http.PostAsJsonAsync("api/auth/login", request);

        if (!response.IsSuccessStatusCode)
            return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        try
        {
            return JsonSerializer.Deserialize<AuthResponse>(responseBody, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }
}
