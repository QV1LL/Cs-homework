using GamesApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GamesApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GamesAppContext>(b =>
        {
            b.UseSqlite("Data Source=GamesApp.sqlite;Pooling=true;");
        });

        return services;
    }
}
