using System.Drawing;

namespace DancingProgressBars.Services;

internal interface IProgressBarsService
{
    bool IsRunning { get; }

    Color AddProgressBar(Action<float> callback);
    void RemoveProgressBar(Action<float> callback);
    Task RunProgressBars();
    void ClearProgressBars();
}
