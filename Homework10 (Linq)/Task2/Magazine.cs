namespace Task2;

internal class Magazine
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public int PageCount { get; set; }
    public DateTime ReleaseDate { get; set; }

    public override string ToString()
        => $"Title: {Title}, Genre: {Genre}, Pages: {PageCount}, Release Date: {ReleaseDate.ToString("yyyy-MM-dd")}";
}
