using LibraryMVC.Domain.Entities;
using LibraryMVC.Domain.ValueObjects;

namespace LibraryMVC.Presentation.Views;

public class MainMenuView
{
    public void DisplayMenu()
    {
        Console.WriteLine("=== Library Menu ===");
        Console.WriteLine("1. Add Author");
        Console.WriteLine("2. List Authors");
        Console.WriteLine("3. Add Book");
        Console.WriteLine("4. List Books");
        Console.WriteLine("5. Update book title");
        Console.WriteLine("6. Delete author");
        Console.WriteLine("7. Delete book");
        Console.WriteLine("8. Exit");
    }

    public void DisplayMessage(string message) => Console.WriteLine(message);

    public void DisplayAuthor(Author author, IEnumerable<Book> authorBooks)
    {
        Console.WriteLine();
        Console.WriteLine($"Name: {author.Name},\nBooks:");
        foreach (Book book in authorBooks) DisplayBook(book, author.Name);
        Console.WriteLine();
    }

    public void DisplayBook(Book book, Name authorName)
    {
        Console.WriteLine($"\n -Title: {book.Title},\n -Author: {authorName}\n");
    }

    public string PromptForString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    public void ClearConsole() => Console.Clear();

    public void WaitForInput()
    {
        var cursorAnimationThread = new Thread(() =>
        {
            Console.WriteLine("Click to continue");

            try
            {
                while (true)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Console.Write(".");
                        Thread.Sleep(500);
                    }

                    Console.SetCursorPosition(Console.CursorLeft - 3, Console.CursorTop);
                    Console.Write("   ");
                    Console.SetCursorPosition(Console.CursorLeft - 3, Console.CursorTop);
                }
            }
            catch { }
        });

        cursorAnimationThread.Start();
        Console.ReadKey();
        cursorAnimationThread.Interrupt();
        ClearConsole();
    }

}