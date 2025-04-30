using App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace App.View;

public sealed partial class MainPage : Page
{
    internal MainPageViewModel ViewModel { get; }

    public MainPage()
    {
        this.InitializeComponent();
        ViewModel = new MainPageViewModel();
    }
}
