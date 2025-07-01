using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace GamesApp.Presentation.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool isVisible = value is bool boolValue && boolValue;
        if (parameter is string param && param.Equals("invert", StringComparison.OrdinalIgnoreCase))
            isVisible = !isVisible;

        return isVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        bool isVisible = value is Visibility visibility && visibility == Visibility.Visible;
        if (parameter is string param && param.Equals("invert", StringComparison.OrdinalIgnoreCase))
            isVisible = !isVisible;

        return isVisible;
    }
}
