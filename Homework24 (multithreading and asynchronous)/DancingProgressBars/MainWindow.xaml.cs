using DancingProgressBars.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace DancingProgressBars;

public partial class MainWindow : Window
{
    private readonly IProgressBarsService _service = new ProgressBarsService();
    private readonly Dictionary<ProgressBar, Action<float>> _progressCallbacks = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var progressBar = new ProgressBar
        {
            Width = 700,
            Height = 25,
            Margin = new Thickness(0, 5, 0, 5),
            Maximum = 100
        };

        ProgressBarsPanel.Children.Add(progressBar);

        Action<float> callback = value =>
            Dispatcher.Invoke(() =>
            {
                var animation = new DoubleAnimation
                {
                    From = progressBar.Value,
                    To = value,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                progressBar.BeginAnimation(ProgressBar.ValueProperty, animation);
            });

        var color = _service.AddProgressBar(callback);

        progressBar.Foreground = new SolidColorBrush(Color.FromArgb(
            color.A, color.R, color.G, color.B));

        _progressCallbacks[progressBar] = callback;
    }

    private void RemoveButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProgressBarsPanel.Children.Count > 0)
        {
            var last = (ProgressBar)ProgressBarsPanel.Children[^1];
            ProgressBarsPanel.Children.Remove(last);
            var callback = _progressCallbacks[last];

            _service.RemoveProgressBar(callback);
            _progressCallbacks.Remove(last);
        }
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        if (!_service.IsRunning)
        {
            _service.ClearProgressBars();

            foreach (var progressBar in _progressCallbacks.Keys)
                Dispatcher.Invoke(() =>
                {
                    var animation = new DoubleAnimation
                    {
                        From = progressBar.Value,
                        To = 0,
                        Duration = TimeSpan.FromMilliseconds(300),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    progressBar.BeginAnimation(ProgressBar.ValueProperty, animation);
                });
        }
    }

    private async void RunButton_Click(object sender, RoutedEventArgs e)
    {
        await _service.RunProgressBars();
    }
}
