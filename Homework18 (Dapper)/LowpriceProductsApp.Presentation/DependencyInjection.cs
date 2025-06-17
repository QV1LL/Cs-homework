using LowpriceProductsApp.Presentation.ViewModels.Pages.ManagePages;
using LowpriceProductsApp.Presentation.ViewModels.Pages.ViewPages;
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
        
        services.AddTransient<ViewCitiesPageViewModel>();
        services.AddTransient<ViewCountriesPageViewModel>();
        services.AddTransient<ViewCustomersPageViewModel>();
        services.AddTransient<ViewPromotionalGoodsPageViewModel>();
        services.AddTransient<ViewSectionsPageViewModel>();

        return services;
    }
}
