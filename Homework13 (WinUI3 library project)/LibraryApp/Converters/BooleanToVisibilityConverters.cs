using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;

namespace LibraryApp.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        try
        {
            return value is bool isVisible && isVisible ? Visibility.Visible : Visibility.Collapsed;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BooleanToVisibilityConverter.Convert failed: {ex.Message}");
            return Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        try
        {
            return value is Visibility visibility && visibility == Visibility.Visible;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BooleanToVisibilityConverter.ConvertBack failed: {ex.Message}");
            return false;
        }
    }
}
