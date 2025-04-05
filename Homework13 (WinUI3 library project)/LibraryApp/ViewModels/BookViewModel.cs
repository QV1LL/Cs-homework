namespace LibraryApp.ViewModels;

public class BookViewModel : ViewModelBase
{
    public string Title { get; }
    public string Description { get; }
    public string AuthorName { get; }

    public BookViewModel(string title, string description, string authorName)
    {
        Title = title;
        Description = description;
        AuthorName = authorName;
    }
}
