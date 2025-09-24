using System.Windows;

namespace ThreeSpecimentsApplication;

public partial class MainWindow : Window
{
    private readonly Semaphore _semaphore;

    public MainWindow()
    {
        _semaphore = new Semaphore(3, 3, "Global\\MyApp");

        InitializeComponent();

        if (!_semaphore.WaitOne(0)) ShowPopupAndShutdown();
    }

    public void ShowPopupAndShutdown()
    {
        InfoPopup.PlacementTarget = this;
        InfoPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
        InfoPopup.IsOpen = true;
        InfoPopup.Closed += (s, e) => Environment.Exit(0);
    }

    protected override void OnClosed(EventArgs e)
    {
        _semaphore.Release();
        _semaphore.Dispose();
        base.OnClosed(e);
    }

    private void ClosePopup_Click(object sender, RoutedEventArgs e)
    {
        InfoPopup.IsOpen = false;
    }
}