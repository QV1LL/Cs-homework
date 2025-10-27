using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using XChat.Shared.Components.Auth;
using XChat.Shared.Models;

namespace XChat.Shared.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
    }

    public async Task<string?> RegisterAsync(User user)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", user);
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            await NotifyAuthentication(token);
            return token;
        }
        return null;
    }

    public async Task<string?> LoginAsync(User user)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", user);
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            await NotifyAuthentication(token);
            return token;
        }
        return null;
    }

    public async Task LogoutAsync()
    {
        await NotifyLogout();
    }

    private async Task NotifyAuthentication(string token)
    {
        var customProvider = (CustomAuthenticationStateProvider)_authStateProvider;
        await customProvider.UpdateAuthenticationState(token);
    }

    private async Task NotifyLogout()
    {
        var customProvider = (CustomAuthenticationStateProvider)_authStateProvider;
        await customProvider.UpdateAuthenticationState(null);
    }
}
