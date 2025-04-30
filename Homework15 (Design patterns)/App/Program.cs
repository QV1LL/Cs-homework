using App.Models.Aggregates;
using App.Models.Entities;

namespace App;

internal static class Program
{
    public static void Main(string[] args)
    {
        TestAddingBooksToManager();
        TestPrintingBooksFromManager();
    }

    private static void TestAddingBooksToManager()
    {
        var bookBuilder1 = new BookBuilder()
            .SetAuthor("Joan Roalling")
            .SetTitle("Harry Potter")
            .SetYear(1990);

        var bookBuilder2 = new BookBuilder()
            .SetTitle("Design patterns")
            .SetAuthor("O`Relly")
            .SetYear(2018);

        var manager = LibraryManager.Instance;
        manager.AddBook(bookBuilder1.Build());
        manager.AddBook(bookBuilder2.Build());
    }

    private static void TestPrintingBooksFromManager()
    {
        var manager = LibraryManager.Instance;
        manager.PrintBooks();
    }
}
