using GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace GamesApp.Presentation.Views.Pages.ManagePages;

public sealed partial class ManageStudiosPage : Page
{
    public readonly ManageStudiosPageViewModel ViewModel;

    public ManageStudiosPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ManageStudiosPageViewModel>();
    }
}
