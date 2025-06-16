using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories.Generic;
using System.Collections.Generic;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories.ConcreateRepositories;

public class CitiesRepository : GenericRepository<City>, ICitiesRepository
{
    public CitiesRepository(ConnectionManager connectionManager)
       : base(
           connectionManager,
           "Cities",
           new List<string> { "Id", "Name"}
       )
    {

    }
}