namespace ResumeAnalyzer.Models;

internal record ResumeDto
(
    string Name,
    int YearsOfExperience,
    string City,
    int SalaryExpectation
)
{
    private const string NAME_PATTERN = "Name:";
    private const string YEARS_OF_EXPERIENCE_PATTERN = "Years of experience:";
    private const string CITY_PATTERN = "City:";
    private const string SALARY_EXPECTATION_PATTERN = "Salary expectation:";

    private static readonly StringComparison Comparison = StringComparison.OrdinalIgnoreCase;

    public static ResumeDto? Parse(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath), "File path cannot be null or empty.");

        if (!File.Exists(filePath))
            return null;

        var lines = File.ReadAllLines(filePath);

        var nameLine = lines.LastOrDefault(
            l => l.Contains(NAME_PATTERN, Comparison)
        );

        var yearsOfExperienceLine = lines.LastOrDefault(
            l => l.Contains(YEARS_OF_EXPERIENCE_PATTERN, Comparison)
        );

        var cityLine = lines.LastOrDefault(
            l => l.Contains(CITY_PATTERN, Comparison)
        );

        var salaryExpectationLine = lines.LastOrDefault(
            l => l.Contains(SALARY_EXPECTATION_PATTERN, Comparison)
        );

        if (nameLine == null || yearsOfExperienceLine == null ||
            cityLine == null || salaryExpectationLine == null)
            return null;

        try
        {
            string name = ExtractValue(nameLine, NAME_PATTERN).Trim();
            if (string.IsNullOrWhiteSpace(name))
                return null;

            string yearsStr = ExtractValue(yearsOfExperienceLine, YEARS_OF_EXPERIENCE_PATTERN).Trim();
            if (!int.TryParse(yearsStr, out int yearsOfExperience) || yearsOfExperience < 0)
                return null;

            string city = ExtractValue(cityLine, CITY_PATTERN).Trim();
            if (string.IsNullOrWhiteSpace(city))
                return null;

            string salaryStr = ExtractValue(salaryExpectationLine, SALARY_EXPECTATION_PATTERN).Trim();
            if (!int.TryParse(salaryStr, out int salaryExpectation) || salaryExpectation < 0)
                return null;

            return new ResumeDto(name, yearsOfExperience, city, salaryExpectation);
        }
        catch
        {
            return null;
        }
    }

    private static string ExtractValue(string line, string pattern)
    {
        return line.Replace(pattern, string.Empty, Comparison).Trim();
    }
}