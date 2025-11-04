using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XChat.UI.Shared.Services;

namespace XChat.UI.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddXChatServices(this IServiceCollection services, IConfiguration? config = null)
    {
        var baseUrl = config?["Api:Url"] ?? string.Empty;

        services.AddHttpClient("ApiClient", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddScoped<AuthService>();
        services.AddScoped<ChatService>();

        return services;
    }
}