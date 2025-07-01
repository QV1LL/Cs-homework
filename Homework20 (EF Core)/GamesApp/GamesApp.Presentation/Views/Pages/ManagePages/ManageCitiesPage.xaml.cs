using GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace GamesApp.Presentation.Views.Pages.ManagePages;

public sealed partial class ManageCitiesPage : Page
{
    public readonly ManageCitiesPageViewModel ViewModel;

    public ManageCitiesPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ManageCitiesPageViewModel>();
    }
}
