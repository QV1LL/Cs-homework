using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesApp.Domain.Entities;

public class Studio
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
}
