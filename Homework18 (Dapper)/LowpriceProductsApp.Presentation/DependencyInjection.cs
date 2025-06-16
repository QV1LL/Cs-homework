using LowpriceProductsApp.Presentation.ViewModels.Pages.ManagePages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LowpriceProductsApp.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ManageCitiesPageViewModel>();
        services.AddTransient<ManageCountriesPageViewModel>();
        services.AddTransient<ManageCustomersPageViewModel>();
        services.AddTransient<ManagePromotionalGoodsPageViewModel>();
        services.AddTransient<ManageSectionsPageViewModel>();

        return services;
    }
}
