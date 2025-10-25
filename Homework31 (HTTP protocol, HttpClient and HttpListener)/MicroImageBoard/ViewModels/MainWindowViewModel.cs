using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicroImageBoard.Helpers;
using MicroImageBoard.Services.SearchSystems;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace MicroImageBoard.ViewModels;

internal partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    public partial ObservableCollection<IImageSearchSystem> SearchSystems { get; set; } = [];

    public ObservableCollection<ImageResultsPerSystem> ImageResultsPerSystem { get; set; } = [];

    [ObservableProperty]
    public partial ImageResultsPerSystem? SelectedImageSystemResults { get; set; }

    [ObservableProperty]
    public partial string Query { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsSearching { get; set; }

    public MainWindowViewModel()
    {
        var customSearchApi = Environment.GetEnvironmentVariable("CUSTOM_SEARCH_API");
        var googleSearchEngineId = Environment.GetEnvironmentVariable("GOOGLE_SEARCH_ENGINE_ID");
        var serpApi = Environment.GetEnvironmentVariable("SERP_API");

        SearchSystems =
        [
            new GoogleCustomSearchImageSystem(customSearchApi ?? string.Empty, 
                                              googleSearchEngineId ?? string.Empty),
            new SerpApiGoogleImageSystem(serpApi ?? string.Empty),
        ];

        foreach (var system in SearchSystems)
        {
            ImageResultsPerSystem.Add(new ImageResultsPerSystem
            {
                System = system,
            });
        }

        SelectedImageSystemResults = ImageResultsPerSystem.FirstOrDefault();
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(Query))
            return;

        IsSearching = true;

        try
        {
            var enabledSystems = SearchSystems.Where(s => s.IsEnabled).ToList();

            foreach (var result in ImageResultsPerSystem)
                result.Results.Clear();

            var searchTasks = enabledSystems.Select((Func<IImageSearchSystem, Task>)(async system =>
            {
                var images = await system.GetImageUrlsAsync((string)this.Query);
                var container = ImageResultsPerSystem.First(r => r.System == system);

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    foreach (var img in images)
                        container.Results.Add((string)img);
                });
            }));

            if (SelectedImageSystemResults == null || !SelectedImageSystemResults.System.IsEnabled)
                SelectedImageSystemResults = ImageResultsPerSystem.FirstOrDefault(r => r.System.IsEnabled);

            await Task.WhenAll(searchTasks);
        }
        finally
        {
            IsSearching = false;
        }
    }

    [RelayCommand]
    private void OpenUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return;

        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch
        {
            Debug.WriteLine($"Cannot open URL: {url}");
        }
    }
}
