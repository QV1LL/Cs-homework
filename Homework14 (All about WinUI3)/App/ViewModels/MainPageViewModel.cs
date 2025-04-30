using System.Collections.Generic;

namespace App.ViewModels;

internal class MainPageViewModel : ViewModelBase
{
    public bool IsLightOn
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            OnPropertyChanged(nameof(LightStateMessage));
        }
    }

    public string LightStateMessage
        => IsLightOn ? "On" : "Off";

    public int Temperature
    {
        get => field;
        set => SetProperty(ref field, value);
    }
    public string RoomName
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public int DaysToServiceLeft
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public string SelectedMode
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public IReadOnlyCollection<string> AvailableModes
        => new string[] { "Winter", "Spring", "Summer", "Autumn" };
}
