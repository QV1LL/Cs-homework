using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LowpriceProductsApp.Domain.Entities;

[Table("Countries")]
public class Country : IEntity
{
    [Column("Id")]
    public Guid? Id { get; set; }
    [Column("Name")]
    public string Name
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            field = value;
        }
    }
}
