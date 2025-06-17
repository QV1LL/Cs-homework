using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories.Generic;
using System.Collections.Generic;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories.ConcreateRepositories;

public class CustomersRepository : GenericRepository<Customer>, ICustomersRepository
{
    private readonly ICitiesRepository _citiesRepository;

    public CustomersRepository(
        ConnectionManager connectionManager, 
        ICitiesRepository citiesRepository)
       : base(
           connectionManager,
           "Customers",
           new List<string> { "Id", "Name", "Gender", "Email", "CityId"}
       )
    { _citiesRepository = citiesRepository; }

    public override IEnumerable<Customer> GetAll()
    {
        var customers = base.GetAll();
        foreach ( var customer in customers )
            customer.City = _citiesRepository.Get(customer.CityId);

        return customers;
    }
}
