using Microsoft.Win32;
using System.IO;
using System.Windows;
using WordFinder.Services;

namespace WordFinder;

public partial class MainWindow : Window
{
    private readonly IWordFinder _wordFinder = new WordFinder.Services.WordFinder();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        if (dialog.ShowDialog() == true)
        {
            FilePathTextBox.Text = dialog.FileName;
        }
    }

    private async void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        string word = WordTextBox.Text.Trim();
        string path = FilePathTextBox.Text.Trim();

        if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(path))
        {
            MessageBox.Show("Please enter a word and select a file.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!File.Exists(path))
        {
            MessageBox.Show("The specified file does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        ResultTextBlock.Text = "Searching...";

        int count = await _wordFinder.GetCountOfWordAppearenceAsync(word, path);

        ResultTextBlock.Text = $"The word \"{word}\" appears {count} time(s).";
    }
}
