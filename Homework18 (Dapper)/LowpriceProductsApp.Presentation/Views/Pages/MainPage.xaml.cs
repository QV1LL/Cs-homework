using LowpriceProductsApp.Presentation.ViewModels.Pages;
using Microsoft.UI.Xaml.Controls;

namespace LowpriceProductsApp.Presentation.Views.Pages;

public sealed partial class MainPage : Page
{
    public MainPageViewModel ViewModel { get; }

    public MainPage()
    {
        InitializeComponent();
        ViewModel = new MainPageViewModel(Frame);
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        => ViewModel.NavigationViewItemClickedCommand.Execute(args.SelectedItem);
}
