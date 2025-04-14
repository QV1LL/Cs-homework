using GeologicalFindingAccountingApp.Presentation.Views;
using LibraryApp.ViewModels.WindowViewModels;
using LibraryApp.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace LibraryApp;

public sealed partial class MainWindow : Window
{
    internal MainWindowViewModel ViewModel { get; }
    public static Frame MainWindowContentFrame { get; private set; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainWindowViewModel(ContentFrame);
        ContentFrame.Navigate(typeof(BooksPage));
        MainWindowContentFrame = ContentFrame;
    }

    private void MainWindowNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        => ViewModel.NavigateCommand.Execute(args.SelectedItem);
}
