using System.Windows;
using WindowsCustomizer.Controls;

namespace WindowsCustomizer;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ContentArea.Content = new AppearanceControl();
    }

    private void Appearance_Click(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = new AppearanceControl();
    }

    private void Performance_Click(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = new PerformanceControl();
    }

    private void Colors_Click(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = new ColorsControl();
    }
}