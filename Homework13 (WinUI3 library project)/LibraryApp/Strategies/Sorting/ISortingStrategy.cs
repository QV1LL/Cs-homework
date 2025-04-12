using System.Collections.Generic;

namespace LibraryApp.Strategies.Sorting;

internal interface ISortingStrategy<T>
{
    IEnumerable<T> Apply(IEnumerable<T> items);
}
