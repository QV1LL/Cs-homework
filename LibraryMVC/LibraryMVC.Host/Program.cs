using LibraryMVC.Presentation.Controllers;
using LibraryMVC.Presentation.Views;

namespace LibraryMVC.Host;

internal static class Program
{
    private const string AuthorsFilePath = "authors.sav";
    private const string BooksFilePath = "books.sav";

    static void Main(string[] args)
    {
        var view = new MainMenuView();
        var controller = new MainMenuController(view, AuthorsFilePath, BooksFilePath);
        
        controller.Run();
    }
}
