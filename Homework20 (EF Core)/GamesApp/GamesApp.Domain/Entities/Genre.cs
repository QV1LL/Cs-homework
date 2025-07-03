using System;
using System.Collections.Generic;

namespace GamesApp.Domain.Entities;

public class Genre : IEntity
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

    public List<Game> Games { get; set; }

    public int GamesCount => Games.Count;
}
