using LowpriceProductsApp.Presentation.ViewModels.Pages.ViewPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace LowpriceProductsApp.Presentation.Views.Pages.ViewPages;

public sealed partial class ViewCitiesPage : Page
{
    public ViewCitiesPageViewModel ViewModel { get; }

    public ViewCitiesPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ViewCitiesPageViewModel>();
    }
}
