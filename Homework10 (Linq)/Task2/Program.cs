namespace Task2;

internal static class Program
{
    static void Main(string[] args)
    {
        // Grok generated array
        var magazines = new Magazine[]
        {
            new Magazine { Title = "Fashion World", Genre = "Fashion", PageCount = 45, ReleaseDate = new DateTime(2022, 5, 15) },
            new Magazine { Title = "Political Herald", Genre = "Politics", PageCount = 32, ReleaseDate = new DateTime(2023, 1, 20) },
            new Magazine { Title = "Gardener", Genre = "Garden and Yard", PageCount = 50, ReleaseDate = new DateTime(2021, 3, 10) },
            new Magazine { Title = "AutoWorld", Genre = "Sports", PageCount = 60, ReleaseDate = new DateTime(2022, 7, 30) },
            new Magazine { Title = "AutoReview", Genre = "Sports", PageCount = 40, ReleaseDate = new DateTime(2023, 9, 15) },
            new Magazine { Title = "Hunting Stories", Genre = "Hunting", PageCount = 35, ReleaseDate = new DateTime(2022, 11, 5) }
        };


        Console.WriteLine($"All page count is bigger than 30: {magazines.All(m => m.PageCount > 30)}");
        Console.WriteLine($"All genres is fashion, politics or sports: {magazines.All(
           m => m.Genre.ToLower() is "fashion" 
        || m.Genre.ToLower() is "politics" 
        || m.Genre.ToLower() is "sports")}");
        Console.WriteLine($"Any genre is Garden and Yard: {magazines.Any(m => m.Genre == "Garden and Yard")}");
        Console.WriteLine($"Any genre is Fishing: {magazines.Any(m => m.Genre == "Fishing")}");
        Console.WriteLine($"Any genre is Hunting: {magazines.Any(m => m.Genre.ToLower() == "hunting")}");
        Console.WriteLine($"First in 2022: {magazines.FirstOrDefault(m => m.ReleaseDate.Year == 2022)}");
        Console.WriteLine($"Last starts with Auto: {magazines.LastOrDefault(m => m.Title.StartsWith("Auto"))}");
    }
}
