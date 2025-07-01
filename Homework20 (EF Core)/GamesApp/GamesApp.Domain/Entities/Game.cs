using GamesApp.Domain.Enums;
using System;
using System.Collections.Generic;

namespace GamesApp.Domain.Entities;

public class Game
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
    public string Description
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(Description));

            field = value;
        }
    }
    public GameType GameType { get; set; }
    public Guid StudioId {  get; set; }
    public Studio Studio { get; set; }
    public List<Genre> Genres { get; set; }
}
