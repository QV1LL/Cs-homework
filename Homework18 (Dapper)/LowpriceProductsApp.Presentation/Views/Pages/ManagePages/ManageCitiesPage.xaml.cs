using LowpriceProductsApp.Presentation.ViewModels.Pages.ManagePages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace LowpriceProductsApp.Presentation.Views.Pages.ManagePages;

public sealed partial class ManageCitiesPage : Page
{
    public readonly ManageCitiesPageViewModel ViewModel;

    public ManageCitiesPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ManageCitiesPageViewModel>();
    }
}
