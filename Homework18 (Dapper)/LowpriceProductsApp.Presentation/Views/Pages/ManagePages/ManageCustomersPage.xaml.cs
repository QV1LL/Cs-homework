using LowpriceProductsApp.Presentation.ViewModels.Pages.ManagePages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace LowpriceProductsApp.Presentation.Views.Pages.ManagePages;

public sealed partial class ManageCustomersPage : Page
{
    public ManageCustomersPageViewModel ViewModel { get; }

    public ManageCustomersPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ManageCustomersPageViewModel>();
    }
}
