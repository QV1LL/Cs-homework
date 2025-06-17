using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories.Generic;
using System.Collections.Generic;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories.ConcreateRepositories;

public class CustomersRepository : GenericRepository<Customer>, ICustomersRepository
{
    private readonly ICountriesRepository _countriesRepository;
    private readonly ICitiesRepository _citiesRepository;

    public CustomersRepository(
        ConnectionManager connectionManager, 
        ICitiesRepository citiesRepository, 
        ICountriesRepository countriesRepository)
       : base(
           connectionManager,
           "Customers",
           new List<string> { "Id", "Name", "Gender", "Email", "CountryId", "CityId"}
       )
    {
        _countriesRepository = countriesRepository;
        _citiesRepository = citiesRepository;
    }

    public override IEnumerable<Customer> GetAll()
    {
        var customers = base.GetAll();

        foreach ( var customer in customers )
        {
            customer.City = _citiesRepository.Get(customer.CityId);
            customer.Country = _countriesRepository.Get(customer.CountryId);
        }

        return customers;
    }
}
