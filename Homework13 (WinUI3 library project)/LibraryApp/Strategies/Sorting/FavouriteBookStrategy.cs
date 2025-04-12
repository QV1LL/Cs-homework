using LibraryApp.ViewModels.PageViewModels;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp.Strategies.Sorting;

internal class FavoriteBooksStrategy : ISortingStrategy<BookViewModel>
{
    public IEnumerable<BookViewModel> Apply(IEnumerable<BookViewModel> books)
    {
        return books.Where(b => b.IsFavorite).OrderBy(b => b.Title);
    }
}
