using ResumeAnalyzer.Models;

namespace ResumeAnalyzer.Services;

internal class ReportGenerator
{
    private readonly IEnumerable<string> _resumeFilePaths;

    public ReportGenerator(IEnumerable<string> resumeFilePaths)
    {
        _resumeFilePaths = resumeFilePaths ?? throw new ArgumentNullException(nameof(resumeFilePaths));
    }

    private List<ResumeDto> LoadResumes()
    {
        return _resumeFilePaths.AsParallel()
                               .Select(ResumeDto.Parse)
                               .Where(r => r != null)
                               .ToList()!;
    }

    public string GenerateReport()
    {
        var resumes = LoadResumes();
        if (!resumes.Any())
            return "No resumes available for analysis.";

        var maxExp = resumes.AsParallel().Max(r => r.YearsOfExperience);
        var mostExperienced = resumes.AsParallel()
                                    .Where(r => r.YearsOfExperience == maxExp)
                                    .Select(r => r.Name)
                                    .ToList();
        string mostExpReport = $"Most Experienced Candidate (Years of Experience: {maxExp}): {string.Join(", ", mostExperienced)}";

        var minExp = resumes.AsParallel().Min(r => r.YearsOfExperience);
        var leastExperienced = resumes.AsParallel()
                                     .Where(r => r.YearsOfExperience == minExp)
                                     .Select(r => r.Name)
                                     .ToList();
        string leastExpReport = $"Least Experienced Candidate (Years of Experience: {minExp}): {string.Join(", ", leastExperienced)}";

        var cityGroups = resumes.AsParallel()
                               .GroupBy(r => r.City)
                               .Where(g => g.Count() > 1)
                               .Select(g => $"{g.Key}: {string.Join(", ", g.Select(r => r.Name))}")
                               .ToList();
        string cityReport = "Candidates from the Same City:\n" + (cityGroups.Any() ? string.Join("\n", cityGroups) : "No groups of candidates from the same city.");

        var minSalary = resumes.AsParallel().Min(r => r.SalaryExpectation);
        var lowestSalaryCandidates = resumes.AsParallel()
                                           .Where(r => r.SalaryExpectation == minSalary)
                                           .Select(r => r.Name)
                                           .ToList();
        string lowSalaryReport = $"Candidate with Lowest Salary Expectation ({minSalary}): {string.Join(", ", lowestSalaryCandidates)}";

        var maxSalary = resumes.AsParallel().Max(r => r.SalaryExpectation);
        var highestSalaryCandidates = resumes.AsParallel()
                                            .Where(r => r.SalaryExpectation == maxSalary)
                                            .Select(r => r.Name)
                                            .ToList();
        string highSalaryReport = $"Candidate with Highest Salary Expectation ({maxSalary}): {string.Join(", ", highestSalaryCandidates)}";

        return string.Join("\n\n", mostExpReport, leastExpReport, cityReport, lowSalaryReport, highSalaryReport);
    }
}