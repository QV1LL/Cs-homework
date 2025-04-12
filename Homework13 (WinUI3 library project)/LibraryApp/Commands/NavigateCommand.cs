using Microsoft.UI.Xaml.Controls;
using System;

namespace LibraryApp.Commands;

internal class NavigateCommand : CommandBase
{
    private readonly Frame _frame;
    private readonly Func<string, Type> _resolvePage;

    public NavigateCommand(Frame frame, Func<string, Type> resolvePage)
    {
        _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        _resolvePage = resolvePage ?? throw new ArgumentNullException(nameof(resolvePage));
    }

    public override bool CanExecute(object parameter) => parameter is string tag && _resolvePage(tag) != null;

    public override void Execute(object parameter)
    {
        if (parameter is string tag)
        {
            try
            {
                Type pageType = _resolvePage(tag);
                if (pageType != null)
                {
                    _frame.Navigate(pageType, tag);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"NavigateCommand.Execute failed: {ex.Message}");
            }
        }
    }
}
