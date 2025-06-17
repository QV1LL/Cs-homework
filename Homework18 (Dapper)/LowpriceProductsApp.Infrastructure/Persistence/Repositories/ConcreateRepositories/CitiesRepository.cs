using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories.Generic;
using System.Collections.Generic;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories.ConcreateRepositories;

public class CitiesRepository : GenericRepository<City>, ICitiesRepository
{
    private readonly ICountriesRepository _countriesRepository;

    public CitiesRepository(
        ConnectionManager connectionManager,
        ICountriesRepository countriesRepository)
       : base(
           connectionManager,
           "Cities",
           new List<string> { "Id", "Name", "CountryId" }
       ) { _countriesRepository = countriesRepository; }

    public override IEnumerable<City> GetAll()
    {
        var cities = base.GetAll();
        foreach (var city in cities) 
            city.Country = _countriesRepository.Get(city.CountryId);

        return cities;
    }
}