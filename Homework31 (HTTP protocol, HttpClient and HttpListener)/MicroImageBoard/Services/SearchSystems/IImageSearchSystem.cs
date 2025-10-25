namespace MicroImageBoard.Services.SearchSystems;

internal interface IImageSearchSystem
{
    public string Name { get; }
    public bool IsEnabled { get; set; }

    Task<IEnumerable<string>> GetImageUrlsAsync(string query);
}
