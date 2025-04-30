using Microsoft.UI.Xaml.Data;
using System;

namespace App.Converters;

internal class DateTimeToDaysConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        try
        {
            if (value is int daysToServiceLeft)
            {
                DateTime targetDate = DateTime.Today.AddDays(daysToServiceLeft);
                return new DateTimeOffset(targetDate);
            }

            return new DateTimeOffset(DateTime.Today);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Convert error: {ex.Message}");
            return new DateTimeOffset(DateTime.Today);
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        try
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                TimeSpan difference = dateTimeOffset.DateTime.Date - DateTime.Today;
                return (int)difference.TotalDays;
            }

            return 0;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ConvertBack error: {ex.Message}");
            return 0;
        }
    }
}
