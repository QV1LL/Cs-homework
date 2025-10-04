using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WindowsCustomizer.Service;

namespace WindowsCustomizer.Controls;

public partial class ColorsControl : UserControl
{
    private Color _menuColor;
    private Color _menuTextColor;

    public ColorsControl()
    {
        InitializeComponent();

        var menuValue = RegistryService.GetValue(@"HKEY_CURRENT_USER\Control Panel\Colors", "Menu")?.ToString();
        if (!string.IsNullOrEmpty(menuValue))
            _menuColor = ParseColor(menuValue);

        var menuTextValue = RegistryService.GetValue(@"HKEY_CURRENT_USER\Control Panel\Colors", "MenuText")?.ToString();
        if (!string.IsNullOrEmpty(menuTextValue))
            _menuTextColor = ParseColor(menuTextValue);

        MenuColorPicker.SelectedColor = _menuColor;
        MenuTextColorPicker.SelectedColor = _menuTextColor;
    }

    private void MenuColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (e.NewValue.HasValue)
            _menuColor = e.NewValue.Value;
    }

    private void MenuTextColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (e.NewValue.HasValue)
            _menuTextColor = e.NewValue.Value;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        RegistryService.SetValue(@"HKEY_CURRENT_USER\Control Panel\Colors", "Menu",
            $"{_menuColor.R} {_menuColor.G} {_menuColor.B}");

        RegistryService.SetValue(@"HKEY_CURRENT_USER\Control Panel\Colors", "MenuText",
            $"{_menuTextColor.R} {_menuTextColor.G} {_menuTextColor.B}");

        MessageBox.Show("Color settings saved! Log off or restart Explorer to apply.");
    }

    private static Color ParseColor(string rgbString)
    {
        try
        {
            var parts = rgbString.Split(' ');
            return Color.FromRgb(
                byte.Parse(parts[0]),
                byte.Parse(parts[1]),
                byte.Parse(parts[2]));
        }
        catch
        {
            return Colors.White;
        }
    }
}
