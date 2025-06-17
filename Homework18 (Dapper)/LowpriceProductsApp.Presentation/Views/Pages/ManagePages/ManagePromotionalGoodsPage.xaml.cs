using LowpriceProductsApp.Presentation.ViewModels.Pages.ManagePages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace LowpriceProductsApp.Presentation.Views.Pages.ManagePages;

public sealed partial class ManagePromotionalGoodsPage : Page
{
    public ManagePromotionalGoodsPageViewModel ViewModel { get; }

    public ManagePromotionalGoodsPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ManagePromotionalGoodsPageViewModel>();
    }
}
