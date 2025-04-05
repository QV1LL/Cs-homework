using System.Collections.ObjectModel;

namespace LibraryApp.ViewModels;

internal class MainPageViewModel : ViewModelBase
{
    public ObservableCollection<BookViewModel> ViewModels { get; }

    public MainPageViewModel()
    {
        ViewModels = new ObservableCollection<BookViewModel>();
    }

    public void AddBook(BookViewModel book) => ViewModels.Add(book);
}
