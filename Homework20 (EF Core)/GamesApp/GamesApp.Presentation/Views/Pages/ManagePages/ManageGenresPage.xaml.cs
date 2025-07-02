using GamesApp.Domain.Entities;
using GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

namespace GamesApp.Presentation.Views.Pages.ManagePages;

public sealed partial class ManageGenresPage : Page
{
    public readonly ManageGenresPageViewModel ViewModel;

    public ManageGenresPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ManageGenresPageViewModel>();
    }
}
