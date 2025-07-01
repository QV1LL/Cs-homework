using GamesApp.Presentation.ViewModels.PageViewModels;
using Microsoft.UI.Xaml.Controls;

namespace GamesApp.Presentation.Views.Pages;

public sealed partial class MainPage : Page
{
    public readonly MainPageViewModel ViewModel;

    public MainPage()
    {
        InitializeComponent();
        ViewModel = new MainPageViewModel(Frame);
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        => ViewModel.NavigationViewItemClickedCommand.Execute(args.SelectedItem);
}
