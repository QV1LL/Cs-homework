using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories.ConcreateRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LowpriceProductsApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ConnectionManager>();

        services.AddScoped<ICitiesRepository, CitiesRepository>();
        services.AddScoped<ICountriesRepository, CountriesRepository>();
        services.AddScoped<IPromotionalGoodsRepository, PromotionalGoodsRepository>();
        services.AddScoped<ISectionsRepository, SectionsRepository>();
        services.AddScoped<ICustomersRepository, CustomersRepository>();

        return services;
    }
}
