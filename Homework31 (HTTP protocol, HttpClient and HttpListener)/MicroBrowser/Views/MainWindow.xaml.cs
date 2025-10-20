using MicroBrowser.ViewModels;
using System.Windows;

namespace MicroBrowser.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Search();
    }
}
