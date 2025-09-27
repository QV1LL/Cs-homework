using Microsoft.Win32;
using System.IO;
using System.Windows;
using TextAnalizer.Services;

namespace TextAnalizer;

public partial class MainWindow : Window
{
    private readonly Dictionary<ITextAnalizeService, Action<int>> services = [];
    public string Result { get; private set; } = string.Empty;
    private CancellationTokenSource? cts;

    public MainWindow()
    {
        InitializeComponent();

        services.Add(new TextSentencesAnalizer(), c => Dispatcher.Invoke(() =>
        {
            Result += $"\nКількість речень: {c}";
            if (DisplayOnScreenRadio.IsChecked == true)
            {
                ReportTextBox.Text = Result.TrimStart();
            }
        }));
        services.Add(new TextSymbolsAnalizer(), c => Dispatcher.Invoke(() =>
        {
            Result += $"\nКількість символів: {c}";
            if (DisplayOnScreenRadio.IsChecked == true)
            {
                ReportTextBox.Text = Result.TrimStart();
            }
        }));
        services.Add(new TextWordsAnalizer(), c => Dispatcher.Invoke(() =>
        {
            Result += $"\nКількість слів: {c}";
            if (DisplayOnScreenRadio.IsChecked == true)
            {
                ReportTextBox.Text = Result.TrimStart();
            }
        }));
        services.Add(new TextAskingSentencesAnalizer(), c => Dispatcher.Invoke(() =>
        {
            Result += $"\nКількість питальних речень: {c}";
            if (DisplayOnScreenRadio.IsChecked == true)
            {
                ReportTextBox.Text = Result.TrimStart();
            }
        }));
        services.Add(new TextExclamationSentencesAnalizer(), c => Dispatcher.Invoke(() =>
        {
            Result += $"\nКількість окличних речень: {c}";
            if (DisplayOnScreenRadio.IsChecked == true)
            {
                ReportTextBox.Text = Result.TrimStart();
            }
        }));
    }

    private async void AnalyzeButton_Click(object sender, RoutedEventArgs e)
    {
        Result = string.Empty;
        ReportTextBox.Text = string.Empty;
        AnalyzeButton.IsEnabled = false;
        StopButton.IsEnabled = true;
        RestartButton.IsEnabled = true;
        cts = new CancellationTokenSource();

        try
        {
            string inputText = InputTextBox.Text;
            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("Будь ласка, введіть текст для аналізу.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                ResetButtons();
                return;
            }

            var tasks = new List<Task>();

            foreach (var service in services)
            {
                tasks.Add(Task.Run(async () =>
                {
                    cts.Token.ThrowIfCancellationRequested();
                    int count = service.Key.GetCount(inputText);
                    await Task.Delay(new Random().Next(500, 800), cts.Token);
                    service.Value(count);
                }, cts.Token));
            }

            await Task.WhenAll(tasks);

            if (DisplayOnScreenRadio.IsChecked == true)
            {
                ReportTextBox.Text = Result.TrimStart();
            }
            else if (SaveToFileRadio.IsChecked == true)
            {
                string filePath = FilePathTextBox.Text;
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    MessageBox.Show("Будь ласка, вкажіть шлях до файлу.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    try
                    {
                        await File.WriteAllTextAsync(filePath, Result.TrimStart());
                        MessageBox.Show($"Звіт успішно збережено до {filePath}", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка при збереженні файлу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            ReportTextBox.Text = "Аналіз зупинено.";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            ResetButtons();
        }
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        cts?.Cancel();
        ResetButtons();
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        cts?.Cancel();
        AnalyzeButton_Click(sender, e);
    }

    private void BrowseFileButton_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            DefaultExt = "txt",
            Title = "Виберіть файл для збереження звіту"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            FilePathTextBox.Text = saveFileDialog.FileName;
        }
    }

    private void ResetButtons()
    {
        AnalyzeButton.IsEnabled = true;
        StopButton.IsEnabled = false;
        RestartButton.IsEnabled = false;
        cts?.Dispose();
        cts = null;
    }
}