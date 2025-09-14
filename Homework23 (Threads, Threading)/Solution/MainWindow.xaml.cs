using Solution.Services;
using System.Windows;

namespace Solution;

public partial class MainWindow : Window
{
    private INumbersGenerator? _primeGenerator;
    private INumbersGenerator? _fibGenerator;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void RunPrimeButton_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(PrimeMinBox.Text, out int min)) min = 2;
        if (!int.TryParse(PrimeMaxBox.Text, out int max)) max = int.MaxValue;
        PrimeList.Items.Clear();

        _primeGenerator?.Stop();
        _primeGenerator = new PrimeNumbersGenerator(min, max);
        _primeGenerator.NextNumberGenerated += n => Dispatcher.Invoke(() => PrimeList.Items.Add(n));
        _primeGenerator.Run();
    }

    private void PausePrimeButton_Click(object sender, RoutedEventArgs e) => _primeGenerator?.Pause();
    private void ResumePrimeButton_Click(object sender, RoutedEventArgs e) => _primeGenerator?.Resume();
    private void StopPrimeButton_Click(object sender, RoutedEventArgs e) => _primeGenerator?.Stop();

    private void RunFibButton_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(FibMinBox.Text, out int min)) min = 2;
        if (!int.TryParse(FibMaxBox.Text, out int max)) max = int.MaxValue;
        FibList.Items.Clear();

        _fibGenerator?.Stop();
        _fibGenerator = new FibonacciNumbersGenerator(min, max);
        _fibGenerator.NextNumberGenerated += n => Dispatcher.Invoke(() => FibList.Items.Add(n));
        _fibGenerator.Run();
    }

    private void PauseFibButton_Click(object sender, RoutedEventArgs e) => _fibGenerator?.Pause();
    private void ResumeFibButton_Click(object sender, RoutedEventArgs e) => _fibGenerator?.Resume();
    private void StopFibButton_Click(object sender, RoutedEventArgs e) => _fibGenerator?.Stop();
}
