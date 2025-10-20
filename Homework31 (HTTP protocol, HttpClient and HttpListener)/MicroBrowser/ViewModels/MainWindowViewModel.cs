using MicroBrowser.Services.SearchSystems;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MicroBrowser.ViewModels;

internal class MainWindowViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ISearchSystem> SearchSystems { get; }
    private ISearchSystem _selectedSystem;
    private string _query;
    private string _currentUrl;

    public MainWindowViewModel()
    {
        SearchSystems = new ObservableCollection<ISearchSystem>
            {
                new GoogleSearchSystem(),
                new BingSearchSystem(),
                new DuckDuckGoSearchSystem()
            };

        SelectedSystem = SearchSystems.FirstOrDefault(s => s.IsEnabled);
    }

    public string Query
    {
        get => _query;
        set
        {
            if (_query != value)
            {
                _query = value;
                OnPropertyChanged(nameof(Query));
            }
        }
    }

    public ISearchSystem SelectedSystem
    {
        get => _selectedSystem;
        set
        {
            if (_selectedSystem != value)
            {
                _selectedSystem = value;
                OnPropertyChanged(nameof(SelectedSystem));
                UpdateSearchUrl();
            }
        }
    }

    public string CurrentUrl
    {
        get => _currentUrl;
        private set
        {
            if (_currentUrl != value)
            {
                _currentUrl = value;
                OnPropertyChanged(nameof(CurrentUrl));
            }
        }
    }

    public void Search()
    {
        if (!string.IsNullOrWhiteSpace(Query) && SelectedSystem != null)
        {
            CurrentUrl = SelectedSystem.GetSearchUrl(Query);
        }
    }

    private void UpdateSearchUrl()
    {
        if (!string.IsNullOrWhiteSpace(Query))
            Search();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
