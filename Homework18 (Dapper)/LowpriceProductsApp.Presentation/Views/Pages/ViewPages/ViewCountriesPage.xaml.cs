using LowpriceProductsApp.Presentation.ViewModels.Pages.ViewPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace LowpriceProductsApp.Presentation.Views.Pages.ViewPages;

public sealed partial class ViewCountriesPage : Page
{
    public ViewCountriesPageViewModel ViewModel { get; }

    public ViewCountriesPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ViewCountriesPageViewModel>();
    }
}
