using System.Windows;
using System.Windows.Controls;
using WindowsCustomizer.Service;

namespace WindowsCustomizer.Controls
{
    public partial class AppearanceControl : UserControl
    {
        public AppearanceControl()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            RegistryService.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                "ColorPrevalence",
                ShowAccentColor.IsChecked == true ? 1 : 0);

            RegistryService.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                "EnableTransparency",
                EnableTransparency.IsChecked == true ? 1 : 0);

            RegistryService.SetValue(
                @"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics",
                "Shell Icon Size",
                IconSizeBox.Text);

            MessageBox.Show("Settings saved! Restart Explorer to apply.");
        }
    }
}
