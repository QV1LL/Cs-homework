using Ookii.Dialogs.Wpf;
using System.Windows;
using WordFinderInDirectory.Domain;
using WordFinderInDirectory.Services;

namespace WordFinderInDirectory;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new VistaFolderBrowserDialog
        {
            Description = "Select a directory to search",
            UseDescriptionForTitle = true
        };

        if (dialog.ShowDialog() == true)
        {
            DirectoryTextBox.Text = dialog.SelectedPath;
        }
    }

    private async void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        string word = WordTextBox.Text.Trim();
        string directory = DirectoryTextBox.Text.Trim();

        if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(directory))
        {
            MessageBox.Show("Enter a word and select a directory.", "Validation Error",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        ResultsList.Items.Clear();
        ResultsList.Items.Add("Searching...");

        bool showOnlyMatches = ShowOnlyMatchesCheckBox.IsChecked ?? false;

        var progress = new Progress<WordSearchResult>(result =>
        {
            if (ResultsList.Items.Count == 1 && ResultsList.Items[0] is string s && s == "Searching...")
                ResultsList.Items.Clear();

            if (showOnlyMatches && result.Count == 0)
                return;

            ResultsList.Items.Add($"Назва файлу: {result.FileName}");
            ResultsList.Items.Add($"Шлях до файлу: {result.FilePath}");
            ResultsList.Items.Add($"Кількість входжень слова: {result.Count}");
            ResultsList.Items.Add(new string('-', 50));
        });

        try
        {
            await Task.Run(() => DirectoryWordFinder.SearchAsync(word, directory, progress));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}