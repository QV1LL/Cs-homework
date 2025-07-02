using GamesApp.Presentation.ViewModels.PageViewModels.ViewPageViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace GamesApp.Presentation.Views.Pages.ViewPages;

public sealed partial class GamesStatisticsPage : Page
{
    public readonly GameStatisticsPageViewModel ViewModel;
    public GamesStatisticsPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<GameStatisticsPageViewModel>();
    }
}
