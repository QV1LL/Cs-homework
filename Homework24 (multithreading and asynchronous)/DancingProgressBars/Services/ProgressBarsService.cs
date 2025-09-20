using DancingProgressBars.Domain;
using System.Drawing;

namespace DancingProgressBars.Services;

internal class ProgressBarsService : IProgressBarsService
{
    public bool IsRunning { get; private set; } = false;

    private readonly Dictionary<Action<float>, ProgressBarMetadata> UpdateCallbackToMetadata = [];
    
    private readonly Random _random = new();
    private readonly Lock _lock = new();

    private event Action<ProgressBarMetadata>? MetadataDeleted;

    public Color AddProgressBar(Action<float> callback)
    {
        var color = Color.FromArgb(_random.Next(256), _random.Next(256), _random.Next(256));
        UpdateCallbackToMetadata[callback] = new ProgressBarMetadata(color);
        return color;
    }

    public void RemoveProgressBar(Action<float> callback)
    {
        var metadata = UpdateCallbackToMetadata[callback];
        UpdateCallbackToMetadata.Remove(callback);
        MetadataDeleted?.Invoke(metadata);
    }

    public async Task RunProgressBars()
    {
        if (IsRunning) return;

        IsRunning = true;
        List<Task> tasks = [];

        foreach (var (callback, metadata) in UpdateCallbackToMetadata)
            tasks.Add(Task.Run(async () => await FillProgressBar(callback, metadata)));

        await Task.WhenAll(tasks);
        IsRunning = false;
    }

    public void ClearProgressBars()
    {
        if (IsRunning) return;

        foreach (var metadata in UpdateCallbackToMetadata.Values)
            metadata.ClearProgress();
    }

    private async Task FillProgressBar(Action<float> callback, ProgressBarMetadata metadata)
    {
        var isProgressBarDeleted = false;
        MetadataDeleted += (deletedMetadata) =>
            isProgressBarDeleted = deletedMetadata == metadata;

        while (metadata.Progress < 100 && !isProgressBarDeleted)
        {
            lock (_lock)
            {
                var increment = (float)_random.NextDouble() * 10;
                metadata.AddProgress(increment);
                callback(metadata.Progress);
            }

            await Task.Delay(_random.Next(100, 500));
        }
    }
}
