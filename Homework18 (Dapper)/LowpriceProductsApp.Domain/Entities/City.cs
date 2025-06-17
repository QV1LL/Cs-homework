using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LowpriceProductsApp.Domain.Entities;

[Table("Cities")]
public class City : IEntity
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
    [Column("CountryId")]
    public Guid CountryId { get; set; }
    public Country Country
    {
        get => field;
        set
        {
            field = value;

            if (value?.Id is Guid countryId)
                CountryId = countryId;
        }
    }
}
