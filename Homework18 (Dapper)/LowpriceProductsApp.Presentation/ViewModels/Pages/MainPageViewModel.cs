using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;

namespace LowpriceProductsApp.Presentation.ViewModels.Pages;

public partial class MainPageViewModel : ObservableObject
{
    private readonly Frame _frame;

    public MainPageViewModel(Frame frame) => _frame = frame;

    [RelayCommand]
    private void NavigationViewItemClicked(object? parameter)
    {
        if (parameter is NavigationViewItem item)
        {
            try
            {
                var page = item.Tag?.ToString();
                var pageTypeString = $"LowpriceProductsApp.Presentation.Views.Pages.{page}";
                var pageType = Type.GetType(pageTypeString);
                _frame?.Navigate(pageType);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
