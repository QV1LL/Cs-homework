using App.Models.Entities;
using System.Threading.Channels;

namespace App.Services;

internal static class PrintBookService
{
    public static void PrintBook(Book book)
    {
        Console.WriteLine($"Title: {book.Title}");
        Console.WriteLine($"Author: {book.Author}");
        Console.WriteLine($"Year: {book.Year}");
    }

    public static void PrintEmpty() => Console.WriteLine();
}
