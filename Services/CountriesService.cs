using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private field
        private readonly PersonsDbContext db;

        //constructor
        public CountriesService(PersonsDbContext countryDbContext)
        {
            db = countryDbContext;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation: countryAddRequest parameter can't be null
            ValidationHelper.ModelValidation(countryAddRequest);

            //Validation: CountryName can't be duplicate
            if (await db.Countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).CountAsync() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountryId
            country.CountryId = Guid.NewGuid();

            //Add country object into _countries
            db.Countries.Add(country);

            await db.SaveChangesAsync();
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse> GetCountryByCountryId(Guid? countryID)
        {
            if (countryID == null)
                return null;

            Country? countryResponseFromList = await db.Countries.FirstOrDefaultAsync(temp => temp.CountryId == countryID);

            if (countryResponseFromList == null)
                return null;

            return countryResponseFromList.ToCountryResponse();
        }
    }
}