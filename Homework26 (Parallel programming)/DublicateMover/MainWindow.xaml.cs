using DublicateMover.Services;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.IO;
using System.Windows;

namespace DublicateMover;

public partial class MainWindow : Window
{
    private CancellationTokenSource? cts;
    private IDuplicateFileService service = new DuplicateFileService();

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        string sourceDir = SourcePathTextBox.Text;
        string destDir = DestPathTextBox.Text;

        if (string.IsNullOrWhiteSpace(sourceDir) || !Directory.Exists(sourceDir))
        {
            MessageBox.Show("Невірний шлях до директорії джерела.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(destDir) || !Directory.Exists(destDir))
        {
            MessageBox.Show("Невірний шлях до директорії приймача.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        ReportTextBox.Text = string.Empty;
        StartButton.IsEnabled = false;
        StopButton.IsEnabled = true;
        RestartButton.IsEnabled = true;
        ProgressBar.Visibility = Visibility.Visible;
        cts = new CancellationTokenSource();

        Action<string> logAction = null;
        if (DisplayOnScreenRadio.IsChecked == true)
        {
            logAction = line => Dispatcher.Invoke(() => ReportTextBox.Text += line + "\n");
        }

        try
        {
            string report = await service.ProcessDirectoryAsync(sourceDir, destDir, logAction, cts.Token);

            if (SaveToFileRadio.IsChecked == true)
            {
                string filePath = ReportFilePathTextBox.Text;
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    MessageBox.Show("Будь ласка, вкажіть шлях до файлу звіту.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    await File.WriteAllTextAsync(filePath, report, cts.Token);
                    MessageBox.Show($"Звіт збережено до {filePath}", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        catch (OperationCanceledException)
        {
            if (DisplayOnScreenRadio.IsChecked == true)
            {
                ReportTextBox.Text += "\nОперацію зупинено.";
            }
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
        StartButton_Click(sender, e);
    }

    private void BrowseSourceButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new VistaFolderBrowserDialog
        {
            Description = "Виберіть директорію джерела",
            UseDescriptionForTitle = true
        };

        if (dialog.ShowDialog(this) == true)
        {
            SourcePathTextBox.Text = dialog.SelectedPath;
        }
    }

    private void BrowseDestButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new VistaFolderBrowserDialog
        {
            Description = "Виберіть директорію приймача",
            UseDescriptionForTitle = true
        };

        if (dialog.ShowDialog(this) == true)
        {
            DestPathTextBox.Text = dialog.SelectedPath;
        }
    }

    private void BrowseReportFileButton_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            DefaultExt = "txt",
            Title = "Виберіть файл для звіту"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            ReportFilePathTextBox.Text = saveFileDialog.FileName;
        }
    }

    private void ResetButtons()
    {
        StartButton.IsEnabled = true;
        StopButton.IsEnabled = false;
        RestartButton.IsEnabled = false;
        ProgressBar.Visibility = Visibility.Collapsed;
        cts?.Dispose();
        cts = null;
    }
}