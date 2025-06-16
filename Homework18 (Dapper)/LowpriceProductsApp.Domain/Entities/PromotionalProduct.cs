using LowpriceProductsApp.Domain.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LowpriceProductsApp.Domain.Entities;

[Table("PromotionalGoods")]
public class PromotionalProduct : IEntity
{
    [Column("Id")]
    public Guid? Id {  get; set; }
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
    public Money StartPrice { get; set; }
    [Column("DiscountPercentage")]
    public int DiscountPercentage
    {
        get => field;
        set
        {
            if (value < 0 || value > 99)
                throw new ArgumentOutOfRangeException(nameof(value));

            field = value;
        }
    }
    [Column("CountryId")]
    public Guid CountryId { get; set; }
    public Country Country { get; set; }
    [Column("SectionId")]
    public Guid SectionId { get; set; }
    public Section Section { get; set; }
    [Column("PromotionStart")]
    public DateTimeOffset PromotionStart { get; set; }
    [Column("PromotionEnd")]
    public DateTimeOffset PromotionEnd { get; set; }
}
