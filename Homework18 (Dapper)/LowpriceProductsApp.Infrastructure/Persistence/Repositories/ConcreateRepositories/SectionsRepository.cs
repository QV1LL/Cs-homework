using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories.Generic;
using System.Collections.Generic;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories.ConcreateRepositories;

public class SectionsRepository : GenericRepository<Section>, ISectionsRepository
{
    public SectionsRepository(ConnectionManager connectionManager)
       : base(
           connectionManager,
           "Sections",
           new List<string> { "Id", "Name" }
       )
    {

    }
}
