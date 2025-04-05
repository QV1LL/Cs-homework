using LibraryApp.View;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace LibraryApp;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        RootFrame.Navigate(typeof(MainPage));
    }
}
