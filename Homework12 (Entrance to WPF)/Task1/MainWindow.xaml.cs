using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Task1;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ColoredPanelsWrapPanel.Width = this.Width * 2 / 3;

        var buttonsColors = new Color[]
        {
            Colors.Navy,
            Colors.Blue,
            Colors.Aqua,
            Colors.Teal,
            Colors.Olive,
            Colors.Green,
            Colors.Lime,
            Colors.Yellow,
            Colors.Red,
            Colors.Maroon,
            Colors.Fuchsia,
            Colors.Purple,
            Colors.Black,
            Colors.Silver,
            Colors.Gray,
            Colors.White,
        };

        var buttons = Enumerable.Range(0, buttonsColors.Length)
            .Select(n =>
            {
                var button = new Button();
                button.Foreground = new SolidColorBrush(buttonsColors[n]);
                button.Background = new SolidColorBrush(Colors.LightGray);
                button.Content = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                    .FirstOrDefault(p => (Color)p.GetValue(null) == buttonsColors[n])?.Name;
                button.Margin = new Thickness(3);
                button.Padding = new Thickness(3);

                return button;
            });

        foreach (var item in buttons)
            ColoredPanelsWrapPanel.Children.Add(item);
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e) 
        => ColoredPanelsWrapPanel.Width = this.Width * 2 / 3;
}