using HorseRaces.Domain;
using HorseRaces.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HorseRaces;

public partial class MainWindow : Window
{
    private readonly IRaceService _raceService = new RaceService();
    private readonly Dictionary<Horse, ProgressBar> _horseBars = [];

    public MainWindow()
    {
        InitializeComponent();
        InitializeRace();
    }

    private void InitializeRace()
    {
        RacePanel.Children.Clear();
        _horseBars.Clear();
        ResultsList.Items.Clear();

        _raceService.InitializeHorses(5);

        foreach (var horse in _raceService.Horses)
        {
            var stack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };

            var nameText = new TextBlock
            {
                Text = horse.Name,
                Width = 80,
                VerticalAlignment = VerticalAlignment.Center
            };

            var progressBar = new ProgressBar
            {
                Width = 600,
                Height = 25,
                Maximum = 100,
                Margin = new Thickness(10, 0, 0, 0),
                Foreground = new SolidColorBrush(Color.FromRgb(horse.Color.R, horse.Color.G, horse.Color.B))
            };

            stack.Children.Add(nameText);
            stack.Children.Add(progressBar);

            RacePanel.Children.Add(stack);
            _horseBars[horse] = progressBar;
        }
    }

    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        if (_raceService.Results.Count > 0) return;

        ResultsList.Items.Clear();

        await _raceService.StartRace(horse =>
        {
            Dispatcher.Invoke(() =>
            {
                var progressBar = _horseBars[horse];

                var animation = new DoubleAnimation
                {
                    From = progressBar.Value,
                    To = horse.DistanceCompleted,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                progressBar.BeginAnimation(ProgressBar.ValueProperty, animation);
            });
        });

        ShowResults();
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        _raceService.ResetRace();
        InitializeRace();
    }

    private void ShowResults()
    {
        var results = _raceService.Results.Select((h, i) => $"{i + 1}. {h.Name}");

        foreach (var line in results)
            ResultsList.Items.Add(line);
    }
}
