using GeologicalFindingAccountingApp.Presentation.Views;
using LibraryApp.Commands;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Windows.Input;

namespace LibraryApp.ViewModels.WindowViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private readonly Frame _frame;

    public ICommand NavigateCommand { get; }

    public MainWindowViewModel(Frame frame)
    {
        _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        NavigateCommand = new RelayCommand(
            (p) =>
            {
                if (p is NavigationViewItem item)
                {
                    var tag = item.Tag?.ToString();
                    var pageTypeString = $"LibraryApp.Views.Pages.{tag}";
                    var pageType = tag == "Settings" ? typeof(SettingsPage) : Type.GetType(pageTypeString);
                    _frame.Navigate(pageType);
                }
            }
        );
    }
}
