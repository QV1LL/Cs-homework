using LowpriceProductsApp.Presentation.ViewModels.Pages.ViewPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace LowpriceProductsApp.Presentation.Views.Pages.ViewPages;

public sealed partial class ViewSectionsPage : Page
{
    public ViewSectionsPageViewModel ViewModel { get; }

    public ViewSectionsPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ViewSectionsPageViewModel>();
    }
}
