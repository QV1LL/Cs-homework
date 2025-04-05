using LibraryApp.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace LibraryApp.View;

public sealed partial class AddBookPage : Page
{
    private Action<BookViewModel>? _addBookCallback;

    public AddBookPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        _addBookCallback = e.Parameter as Action<BookViewModel>;
        DataContext = new AddBookViewModel(_addBookCallback!);
    }

    private void SaveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        (DataContext as AddBookViewModel)?.AddBook();
        Frame.GoBack();
    }
}
