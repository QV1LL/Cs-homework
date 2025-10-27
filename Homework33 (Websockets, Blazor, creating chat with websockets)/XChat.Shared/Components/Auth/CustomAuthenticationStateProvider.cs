using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace XChat.Shared.Components.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService? _localStorage;
    private ClaimsPrincipal? _currentUser;

    public CustomAuthenticationStateProvider(ILocalStorageService? localStorage = null)
    {
        _localStorage = localStorage;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_currentUser ?? new ClaimsPrincipal(new ClaimsIdentity())));
    }

    public async Task UpdateAuthenticationState(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            _currentUser = null;
            await _localStorage?.RemoveItemAsync("authToken");
        }
        else
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, token) }, "jwt");
            _currentUser = new ClaimsPrincipal(identity);
            await _localStorage?.SetItemAsync("authToken", token);
        }
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
