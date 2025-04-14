using LibraryApp.ViewModels.EntityViewModels;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp.Strategies.Sorting;

internal class BooksByGenreStrategy : ISortingStrategy<BookViewModel>
{
    public IEnumerable<BookViewModel> Apply(IEnumerable<BookViewModel> books) 
        => books.OrderBy(b => b.Genre).ThenBy(b => b.Title);
}