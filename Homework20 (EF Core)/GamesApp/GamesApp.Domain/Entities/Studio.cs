using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesApp.Domain.Entities;

public class Studio : IEntity
{
    public Guid? Id { get; set; }
    public string Name
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(Name));

            field = value;
        }
    }
    public List<Game> Games { get; set; } = new ();
    public List<City> Cities { get; set; } = new ();

    public string AllCities => string.Join(", ", Cities.Select(c => c.Name));
    public string DominantGameGenres => string.Join(", ", GetDominantGameGenres().Select(g => g.Name));

    private IEnumerable<Genre> GetDominantGameGenres()
    {
        if (Games == null || Games.Count == 0)
            return Enumerable.Empty<Genre>();

        var genreCounts = Games
            .SelectMany(g => g.Genres ?? [])
            .GroupBy(genre => genre.Id)
            .Select(group => new
            {
                Genre = group.First(),
                Count = group.Count()
            })
            .ToList();

        if (genreCounts.Count == 0)
            return Enumerable.Empty<Genre>();

        var maxCount = genreCounts.Max(g => g.Count);

        return genreCounts
            .Where(g => g.Count == maxCount)
            .Select(g => g.Genre);
    }
}
