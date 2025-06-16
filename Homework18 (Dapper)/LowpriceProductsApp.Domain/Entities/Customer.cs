using LowpriceProductsApp.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace LowpriceProductsApp.Domain.Entities;

[Table("Customers")]
public class Customer : IEntity
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
    [Column("Gender")]
    public Gender Gender { get; set; }
    [Column("Email")]
    public string Email
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email не може бути порожнім");

            if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Невірний формат email");

            field = value;
        }
    }
    [Column("CountryId")]
    public Guid CountryId { get; set; }
    public Country Country { get; set; }
    [Column("CityId")]
    public Guid CityId { get; set; }
    public City City { get; set; }
}
