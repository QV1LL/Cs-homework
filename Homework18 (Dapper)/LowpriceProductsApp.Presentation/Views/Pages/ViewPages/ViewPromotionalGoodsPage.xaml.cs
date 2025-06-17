using LowpriceProductsApp.Presentation.ViewModels.Pages.ViewPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace LowpriceProductsApp.Presentation.Views.Pages.ViewPages;

public sealed partial class ViewPromotionalGoodsPage : Page
{
    public ViewPromotionalGoodsPageViewModel ViewModel { get; }

    public ViewPromotionalGoodsPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ViewPromotionalGoodsPageViewModel>();
    }
}
