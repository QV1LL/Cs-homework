using LibraryMVC.Domain.Entities;
using LibraryMVC.Domain.ValueObjects;
using LibraryMVC.Infrastructure.Persistence;
using LibraryMVC.Presentation.Views;

namespace LibraryMVC.Presentation.Controllers;

public class MainMenuController
{
    private readonly MainMenuView _view;
    private readonly FileRepository<Author> _authorRepository;
    private readonly FileRepository<Book> _bookRepository;
    private bool _running;

    public MainMenuController(MainMenuView view, string authorsFilePath, string booksFilePath)
    {
        _view = view;
        _authorRepository = new FileRepository<Author>(authorsFilePath);
        _bookRepository = new FileRepository<Book>(booksFilePath);
        _running = true;
    }

    public void Run()
    {
        while (_running)
        {
            _view.ClearConsole();
            _view.DisplayMenu();
            string choice = _view.PromptForString("Enter a command: ");

            switch (choice)
            {
                case "1":
                    AddAuthor(GetNewName());
                    break;
                case "2":
                    ListAuthors();
                    break;
                case "3":
                    AddBook();
                    break;
                case "4":
                    ListBooks();
                    break;
                case "5":
                    UpdateBookTitle();
                    break;
                case "6":
                    DeleteAuthor();
                    break;
                case "7":
                    DeleteBook();
                    break;
                case "8":
                    _running = false;
                    _view.DisplayMessage("Exiting...");
                    break;
                default:
                    _view.DisplayMessage("Invalid choice. Try again.");
                    _view.WaitForInput();
                    break;
            }
        }

        this._authorRepository.Dispose();
        this._bookRepository.Dispose();
    }

    private void AddAuthor(Name name)
    {
        var author = new Author(name);
        _authorRepository.Create(author);
        _view.DisplayMessage("Author added successfully!");
    }

    private void ListAuthors()
    {
        var authors = _authorRepository.Get();
        if (authors.Count == 0)
            _view.DisplayMessage("No authors found.");
        else
            foreach (var author in authors)
                _view.DisplayAuthor(author, this._bookRepository.Get(pageSize: int.MaxValue).Where(b => b.AuthorId == author.Id).ToList());

        _view.WaitForInput();
    }

    private void AddBook()
    {
        string title = _view.PromptForString("Enter book title: ");
        string authorName = _view.PromptForString("Enter author name: ");

        Author? author = this._authorRepository.Get(pageSize: int.MaxValue).Find(a => a.Name.ToString().Contains(authorName));

        if (author == null)
        {
            _view.DisplayMessage("Author wasn`t found, creating new...");
            var name = GetNewName();
            AddAuthor(name);
            author = this._authorRepository.Get(pageSize: int.MaxValue).Find(a => a.Name == name);
        }

        var book = new Book(title, author.Id);
        author.AddBook(book.Id);

        _bookRepository.Create(book);
        _authorRepository.Update(a => a.Id == author.Id, author);
        _view.DisplayMessage("Book added successfully!");
    }

    private void ListBooks()
    {
        var books = _bookRepository.Get();
        if (books.Count == 0)
            _view.DisplayMessage("No books found.");
        else
            foreach (var book in books)
                _view.DisplayBook(book, this._authorRepository.Get().Find(a => a.Id == book.AuthorId).Name);
        
        _view.WaitForInput();
    }

    private void UpdateBookTitle()
    {
        Book? book = _bookRepository.Get().Find(b => b.Title == _view.PromptForString("Enter a title of book which will updated: "));

        if (book != null)
        {
            string updatedTitle = _view.PromptForString("Enter a updated title: ");
            book.Title = updatedTitle;
            this._bookRepository.Update(b => b.Id == book.Id, book);
        }
        else
        {
            _view.DisplayMessage("Book wasn`t found");
            _view.WaitForInput();
        }
    }

    private void DeleteBook()
    {
        var book = _bookRepository.Get().Find(b => b.Title == _view.PromptForString("Enter a title of book which will deleted: "));

        if (book != null)
        {
            var author = this._authorRepository.Get().Find(a => a.Id == book.Id);
            author?.RemoveBook(book.Id);
            _authorRepository.Update(a => a.Id == author.Id, author);
            _bookRepository.Delete(b => b.Id == book.Id);
        }
        else
        {
            _view.DisplayMessage("Book wasn`t found");
            _view.WaitForInput();
        }
            
    }

    private void DeleteAuthor()
    {
        _view.DisplayMessage("Enter a name of author who will deleted: ");
        var name = GetNewName();
        var author = _authorRepository.Get().Find(a => a.Name == name);

        if (author != null)
        {
            _bookRepository.Delete(b => author.IsAuthor(b.Id));
            _authorRepository.Delete(a => a.Id == author.Id);
        }
        else
        {
            _view.DisplayMessage("Author wasn`t found");
            _view.WaitForInput();
        }
            
    }

    private Name GetNewName()
    {
        string firstName = _view.PromptForString("Enter author first name: ");
        string lastName = _view.PromptForString("Enter author last name: ");
        string? middleName = _view.PromptForString("Enter author middle name (optional): ");

        return new(firstName, lastName, middleName);
    }
}
