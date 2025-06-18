using SpainChampionship.Domain.Entities;

namespace SpainChampionship.Presentation.Services;

internal static class PrintService
{
    public static void PrintTeams(IEnumerable<Team> teams)
    {
        Console.WriteLine("===== Teams =====");
        foreach (var team in teams)
        {
            Console.WriteLine($"Name: {team.Name}");
            Console.WriteLine($"City: {team.City}");
            Console.WriteLine($"Victories: {team.CountOfVictories}");
            Console.WriteLine($"Defeats: {team.CountOfDefeats}");
            Console.WriteLine($"Draws: {team.CountOfDraws}");
            Console.WriteLine($"Goals: {team.CountOfGoals}");
            Console.WriteLine($"Skipped goals: {team.CountOfSkippedGoals}");
            Console.WriteLine("---------------------------");
        }
    }
}
