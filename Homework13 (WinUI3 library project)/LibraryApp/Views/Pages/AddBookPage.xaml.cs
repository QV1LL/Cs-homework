using LibraryApp.ViewModels.PageViewModels;
using Microsoft.UI.Xaml.Controls;

namespace LibraryApp.Views;

public sealed partial class AddBookPage : Page
{
    public AddBookPage()
    {
        this.InitializeComponent();
        this.DataContext = new AddBookPageViewModel();
    }
}
