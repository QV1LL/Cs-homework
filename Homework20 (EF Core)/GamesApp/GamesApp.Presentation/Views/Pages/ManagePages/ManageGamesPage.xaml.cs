using GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace GamesApp.Presentation.Views.Pages.ManagePages;

public sealed partial class ManageGamesPage : Page
{
    public readonly ManageGamesPageViewModel ViewModel;

    public ManageGamesPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ManageGamesPageViewModel>();
    }
}
