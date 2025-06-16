using LowpriceProductsApp.Presentation.ViewModels.Pages.ManagePages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace LowpriceProductsApp.Presentation.Views.Pages.ManagePages;

public sealed partial class ManageSectionsPage : Page
{
    public ManageSectionsPageViewModel ViewModel { get; }

    public ManageSectionsPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ManageSectionsPageViewModel>();

    }
}
