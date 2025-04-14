using LibraryApp.ViewModels.EntityViewModels;
using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp.Strategies.Sorting;

internal class FavoriteBooksStrategy : ISortingStrategy<BookViewModel>
{
    public IEnumerable<BookViewModel> Apply(IEnumerable<BookViewModel> books)
        => books.Where(b => b.IsFavourite == Visibility.Visible).OrderBy(b => b.Title);
}
