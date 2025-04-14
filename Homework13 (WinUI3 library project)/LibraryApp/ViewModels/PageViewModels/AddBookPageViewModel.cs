using LibraryApp.Commands;
using LibraryApp.Domain;
using System;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace LibraryApp.ViewModels.PageViewModels;

internal class AddBookPageViewModel : ViewModelBase
{
    public string Title
    {
        get => field;
        set => SetProperty(ref field, value);
    }
    public string Author
    {
        get => field;
        set => SetProperty(ref field, value);
    }
    public string Genre
    {
        get => field;
        set => SetProperty(ref field, value);
    }
    public bool IsFavourite
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    private string? CoverImagePath { get; set; }
    
    public ICommand AddBookCommand { get; }
    public ICommand PickImageFileCommand { get; }
    public ICommand CloseDialogCommand { get; }

    public event EventHandler ShowDialogRequested;

    public AddBookPageViewModel()
    {
        AddBookCommand = new RelayCommand(
            async (p) =>
            {
                try
                {
                    var book = new Book(Title, Author, Genre, CoverImagePath, IsFavourite);
                    App.Books.Add(book);
                }
                catch (Exception e)
                {
                    ShowDialogRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        );

        PickImageFileCommand = new RelayCommand(
            async (p) =>
            {
                var picker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var file = await picker.PickSingleFileAsync();
                if (file != null) CoverImagePath = file.Path;
            }
        );
        CloseDialogCommand = new RelayCommand((p) => { });
    }
}
