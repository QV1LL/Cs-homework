using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories.Generic;
using System.Collections.Generic;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories.ConcreateRepositories;

public class PromotionalGoodsRepository : GenericRepository<PromotionalProduct>, IPromotionalGoodsRepository
{
    private readonly ICountriesRepository _countriesRepository;
    private readonly ISectionsRepository _sectionsRepository;

    public PromotionalGoodsRepository(
        ConnectionManager connectionManager,
        ICountriesRepository countriesRepository,
        ISectionsRepository sectionsRepository)
       : base(
           connectionManager,
           "PromotionalGoods",
           new List<string> { "Id", "Name", "StartPriceWholePart", "StartPriceDecimalPart", "DiscountPercentage", 
                              "CountryId", "SectionId", "PromotionStart", "PromotionEnd" }
       )
    {
        _countriesRepository = countriesRepository;
        _sectionsRepository = sectionsRepository;
    }

    public override IEnumerable<PromotionalProduct> GetAll()
    {
        var products = base.GetAll();

        foreach (var product in products)
        {
            product.Section = _sectionsRepository.Get(product.SectionId);
            product.Country = _countriesRepository.Get(product.CountryId);
        }

        return products;
    }
}