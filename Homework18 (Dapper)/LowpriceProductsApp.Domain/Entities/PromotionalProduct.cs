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

    [Column("StartPriceWholePart")]
    public int StartPriceWholePart { get; set; }
    [Column("StartPriceDecimalPart")]
    public int StartPriceDecimalPart { get; set; }
    public Money StartPrice
    {
        get
        {
            if (field == null)
                field = new Money(StartPriceWholePart, StartPriceDecimalPart);

            return field;
        }
        set
        {
            field = value;

            StartPriceDecimalPart = field.DecimalPart;
            StartPriceWholePart = field.WholePart;
        }
    }
    public Money PriceWithDiscount 
        => StartPrice * (100 - DiscountPercentage) / 100;
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
    public Country Country
    {
        get => field;
        set
        {
            field = value;

            if (value.Id is Guid countryId)
                CountryId = countryId;
        }
    }
    [Column("SectionId")]
    public Guid SectionId { get; set; }
    public Section Section
    {
        get => field;
        set
        {
            field = value;

            if (value.Id is Guid sectionId)
                SectionId = sectionId;
        }
    }
    [Column("PromotionStart")]
    public DateTimeOffset PromotionStart { get; set; }
    [Column("PromotionEnd")]
    public DateTimeOffset PromotionEnd { get; set; }
}
