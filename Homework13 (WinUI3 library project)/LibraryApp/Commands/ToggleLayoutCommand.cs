using Microsoft.UI.Xaml.Controls;
using System;

namespace LibraryApp.Commands;

internal class ToggleLayoutCommand : CommandBase
{
    private readonly Action<Layout> setLayout;
    private readonly Action<string> setButtonText;
    private Layout currentLayout;

    public ToggleLayoutCommand(Action<Layout> setLayout, Action<string> setButtonText, Layout initialLayout)
    {
        this.setLayout = setLayout ?? throw new ArgumentNullException(nameof(setLayout));
        this.setButtonText = setButtonText ?? throw new ArgumentNullException(nameof(setButtonText));
        this.currentLayout = initialLayout ?? throw new ArgumentNullException(nameof(initialLayout));
    }

    public override void Execute(object parameter)
    {
        if (currentLayout is UniformGridLayout)
        {
            currentLayout = new StackLayout();
            setLayout(currentLayout);
            setButtonText("Switch to Grid Layout");
        }
        else
        {
            currentLayout = new UniformGridLayout
            {
                MaximumRowsOrColumns = 3,
                MinItemWidth = 200,
                MinItemHeight = 300
            };
            setLayout(currentLayout);
            setButtonText("Switch to Stack Layout");
        }
    }

    public event EventHandler CanExecuteChanged;
}
