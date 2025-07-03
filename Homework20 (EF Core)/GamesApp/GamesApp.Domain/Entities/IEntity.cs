using System;

namespace GamesApp.Domain.Entities;

public interface IEntity
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
}
