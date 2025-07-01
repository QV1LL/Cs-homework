using GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;
using GamesApp.Presentation.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GamesApp.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ManageCitiesPageViewModel>();
        services.AddTransient<ManageGamesPageViewModel>();
        services.AddTransient<ManageGenresPageViewModel>();
        services.AddTransient<ManageStudiosPageViewModel>();

        //services.AddTransient<ViewCitiesPageViewModel>();
        //services.AddTransient<ViewGamesPageViewModel>();
        //services.AddTransient<ViewGenresPageViewModel>();
        //services.AddTransient<ViewStudiosPageViewModel>();

        services.AddSingleton<MainWindow>();

        return services;
    }
}
