using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories.Generic;
using System.Collections.Generic;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories.ConcreateRepositories;

public class PromotionalGoodsRepository : GenericRepository<PromotionalProduct>, IPromotionalGoodsRepository
{
    public PromotionalGoodsRepository(ConnectionManager connectionManager)
       : base(
           connectionManager,
           "PromotionalGoods",
           new List<string> { "Id", "Name", "StartPriceWholePart", "StartPriceDecimalPart", "DiscountPercentage", 
                              "CountryId", "SectionId", "PromotionStart", "PromotionEnd" }
       )
    {

    }
}