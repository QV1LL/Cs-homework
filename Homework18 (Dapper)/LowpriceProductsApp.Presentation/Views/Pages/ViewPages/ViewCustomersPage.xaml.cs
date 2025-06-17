using LowpriceProductsApp.Presentation.ViewModels.Pages.ViewPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace LowpriceProductsApp.Presentation.Views.Pages.ViewPages;

public sealed partial class ViewCustomersPage : Page
{
    public ViewCustomersPageViewModel ViewModel { get; }

    public ViewCustomersPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ViewCustomersPageViewModel>();
    }
}
