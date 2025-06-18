using SpainChampionship.Domain.Entities;
using SpainChampionship.Infrastructure.Persistence;

namespace SpainChampionship.Infrastructure.Services;

public class DataSeeder
{
    private readonly SpainChampionshipContext _context;

    public DataSeeder(SpainChampionshipContext context)
    {
        _context = context;
    }

    public void SeedTeams()
    {
        if (_context.Teams.Any()) return;

        var teams = new List<Team>
        {
            new Team { Name = "Real Madrid", City = "Madrid", CountOfVictories = 25, CountOfDefeats = 3, CountOfDraws = 25, CountOfGoals = 75, CountOfSkippedGoals = 20 },
            new Team { Name = "Barcelona", City = "Barcelona", CountOfVictories = 23, CountOfDefeats = 4, CountOfDraws = 23, CountOfGoals = 70, CountOfSkippedGoals = 22 },
            new Team { Name = "Atletico Madrid", City = "Madrid", CountOfVictories = 20, CountOfDefeats = 5, CountOfDraws = 20, CountOfGoals = 65, CountOfSkippedGoals = 25 },
            new Team { Name = "Sevilla", City = "Seville", CountOfVictories = 18, CountOfDefeats = 7, CountOfDraws = 18, CountOfGoals = 60, CountOfSkippedGoals = 28 },
            new Team { Name = "Valencia", City = "Valencia", CountOfVictories = 15, CountOfDefeats = 10, CountOfDraws = 15, CountOfGoals = 55, CountOfSkippedGoals = 35 },
            new Team { Name = "Villarreal", City = "Villarreal", CountOfVictories = 17, CountOfDefeats = 9, CountOfDraws = 17, CountOfGoals = 58, CountOfSkippedGoals = 30 },
            new Team { Name = "Real Sociedad", City = "San Sebastián", CountOfVictories = 19, CountOfDefeats = 8, CountOfDraws = 19, CountOfGoals = 62, CountOfSkippedGoals = 27 },
            new Team { Name = "Athletic Bilbao", City = "Bilbao", CountOfVictories = 16, CountOfDefeats = 11, CountOfDraws = 16, CountOfGoals = 57, CountOfSkippedGoals = 33 },
            new Team { Name = "Real Betis", City = "Seville", CountOfVictories = 14, CountOfDefeats = 12, CountOfDraws = 14, CountOfGoals = 52, CountOfSkippedGoals = 36 },
            new Team { Name = "Espanyol", City = "Barcelona", CountOfVictories = 12, CountOfDefeats = 14, CountOfDraws = 12, CountOfGoals = 50, CountOfSkippedGoals = 40 },
            new Team { Name = "Getafe", City = "Getafe", CountOfVictories = 11, CountOfDefeats = 15, CountOfDraws = 11, CountOfGoals = 48, CountOfSkippedGoals = 42 },
            new Team { Name = "Granada", City = "Granada", CountOfVictories = 13, CountOfDefeats = 14, CountOfDraws = 13, CountOfGoals = 49, CountOfSkippedGoals = 41 },
            new Team { Name = "Osasuna", City = "Pamplona", CountOfVictories = 14, CountOfDefeats = 13, CountOfDraws = 14, CountOfGoals = 51, CountOfSkippedGoals = 39 },
            new Team { Name = "Celta Vigo", City = "Vigo", CountOfVictories = 12, CountOfDefeats = 15, CountOfDraws = 12, CountOfGoals = 47, CountOfSkippedGoals = 43 },
            new Team { Name = "Mallorca", City = "Palma", CountOfVictories = 10, CountOfDefeats = 16, CountOfDraws = 10, CountOfGoals = 44, CountOfSkippedGoals = 45 },
            new Team { Name = "Rayo Vallecano", City = "Madrid", CountOfVictories = 11, CountOfDefeats = 15, CountOfDraws = 11, CountOfGoals = 46, CountOfSkippedGoals = 44 },
            new Team { Name = "Alaves", City = "Vitoria-Gasteiz", CountOfVictories = 9, CountOfDefeats = 18, CountOfDraws = 9, CountOfGoals = 42, CountOfSkippedGoals = 48 },
            new Team { Name = "Cadiz", City = "Cadiz", CountOfVictories = 8, CountOfDefeats = 19, CountOfDraws = 8, CountOfGoals = 40, CountOfSkippedGoals = 50 },
            new Team { Name = "Elche", City = "Elche", CountOfVictories = 7, CountOfDefeats = 20, CountOfDraws = 7, CountOfGoals = 38, CountOfSkippedGoals = 52 },
            new Team { Name = "Levante", City = "Valencia", CountOfVictories = 6, CountOfDefeats = 21, CountOfDraws = 6, CountOfGoals = 36, CountOfSkippedGoals = 54 }
        };


        _context.Teams.AddRange(teams);
        _context.SaveChanges();
    }
}
