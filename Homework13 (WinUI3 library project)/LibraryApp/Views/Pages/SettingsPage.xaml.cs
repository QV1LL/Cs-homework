using LibraryApp;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

namespace GeologicalFindingAccountingApp.Presentation.Views
{
    public sealed partial class SettingsPage : Page
    {

        public SettingsPage()
        {
            this.InitializeComponent();

            var currentTheme = (App.MainWindow?.Content as FrameworkElement)!.RequestedTheme;
            ThemeComboBox.SelectedItem = ThemeComboBox.Items
                .FirstOrDefault(i => (i as ComboBoxItem)!.Tag.ToString() == currentTheme.ToString());
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string? theme = selectedItem.Tag?.ToString();

                (App.MainWindow?.Content as FrameworkElement)!.RequestedTheme = theme switch
                {
                    "Light" => ElementTheme.Light,
                    "Dark" => ElementTheme.Dark,
                    _ => ElementTheme.Default
                };
            }
        }
    }
}