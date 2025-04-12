using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
using System;
using LibraryApp.Commands;
using LibraryApp.Views.Pages;

namespace LibraryApp.ViewModels.WindowViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    public ICommand NavigateCommand { get; }

    public MainWindowViewModel(Frame frame)
    {
        NavigateCommand = new NavigateCommand(frame, ResolvePageType);
    }

    private Type ResolvePageType(string tag)
    {
        return tag switch
        {
            "BooksPage" => typeof(BooksPage),
            _ => null
        };
    }
}
