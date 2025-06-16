using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories.Generic;
using System.Collections.Generic;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories.ConcreateRepositories;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    public CountriesRepository(ConnectionManager connectionManager)
       : base(
           connectionManager,
           "Countries",
           new List<string> { "Id", "Name" }
       )
    {

    }
}
