using System.Windows;
using System.Windows.Controls;
using WindowsCustomizer.Service;

namespace WindowsCustomizer.Controls;

public partial class PerformanceControl : UserControl
{
    private int _menuDelay;

    public PerformanceControl()
    {
        InitializeComponent();

        var value = RegistryService.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay");
        _menuDelay = value != null ? int.Parse(value.ToString() ?? string.Empty) : 400;

        MenuDelaySlider.Value = _menuDelay;
        MenuDelayValue.Text = _menuDelay.ToString();

        var thumbCacheValue = RegistryService.GetValue(
            @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            "DisableThumbnailCache");
        DisableThumbnails.IsChecked = thumbCacheValue != null && thumbCacheValue.ToString() == "1";
    }

    private void MenuDelaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _menuDelay = (int)e.NewValue;
        MenuDelayValue.Text = _menuDelay.ToString();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        RegistryService.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", _menuDelay);

        RegistryService.SetValue(
            @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            "DisableThumbnailCache",
            DisableThumbnails.IsChecked == true ? 1 : 0);

        MessageBox.Show("Performance settings saved! Restart Explorer to apply.");
    }
}
