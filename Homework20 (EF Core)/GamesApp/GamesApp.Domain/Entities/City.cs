using System;
using System.Collections.Generic;

namespace GamesApp.Domain.Entities;

public class City : IEntity
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

    public string Country
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(Country));

            field = value;
        }
    }

    public List<Studio> Studios { get; set; } = new ();
}
