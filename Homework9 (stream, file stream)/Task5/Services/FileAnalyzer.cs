namespace Task5.Services;

public class FileAnalyzer
{
    private readonly string _filePath;
    private List<int> _analyzedNumbers = null;

    public FileAnalyzer(string filePath)
        => _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

    public List<int> GetAndAnalyzeNumbers(Func<int, bool> predicate)
    {
        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"File {_filePath} not found.");

        try
        {
            string[] lines = File.ReadAllLines(_filePath);
            int[] numbers = lines.Select(int.Parse).ToArray();

            this._analyzedNumbers = numbers.Where(predicate).ToList();
            return this._analyzedNumbers;
        }
        catch (FormatException)
        {
            throw new FormatException("Incorrect format");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}");
        }
    }

    public void SaveToFile(string outputFilePath)
        => File.WriteAllLines(outputFilePath, this._analyzedNumbers.Select(n => n.ToString()));
}