using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories.Generic;
using System.Collections.Generic;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories.ConcreateRepositories;

public class CustomersRepository : GenericRepository<Customer>, ICustomersRepository
{
    public CustomersRepository(ConnectionManager connectionManager)
       : base(
           connectionManager,
           "Customers",
           new List<string> { "Id", "Name", "Gender", "Email", "CountryId", "CityId"}
       )
    {

    }
}
