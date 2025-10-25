using MicroImageBoard.Services.SearchSystems;
using System.Collections.ObjectModel;

namespace MicroImageBoard.Helpers;

internal class ImageResultsPerSystem
{
    public IImageSearchSystem System { get; set; }
    public ObservableCollection<string> Results { get; set; } = new();
}
