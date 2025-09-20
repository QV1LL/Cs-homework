using System.Drawing;

namespace DancingProgressBars.Domain;

internal class ProgressBarMetadata
{
    public float Progress { get; private set; }
    public Color Color { get; }

    public ProgressBarMetadata(Color color)
    {
        Color = color;
        Progress = 0;
    }

    public void AddProgress(float progress)
    {
        if (progress <= 0) return;
        if (Progress + progress >= 100)
        {
            Progress = 100;
            return;
        }

        Progress += progress;
    }

    public void ClearProgress() => Progress = 0;
}
