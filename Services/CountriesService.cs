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
        private readonly PersonDbContext db;

        //constructor
        public CountriesService(PersonDbContext countryDbContext)
        {
            db = countryDbContext;
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation: countryAddRequest parameter can't be null
            ValidationHelper.ModelValidation(countryAddRequest);

            //Validation: CountryName can't be duplicate
            if (db.Countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountryId
            country.CountryId = Guid.NewGuid();

            //Add country object into _countries
            db.Countries.Add(country);

            db.SaveChanges();
            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return db.Countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse GetCountryByCountryId(Guid? countryID)
        {
            if (countryID == null)
                return null;

            Country? countryResponseFromList =  db.Countries.FirstOrDefault(temp => temp.CountryId == countryID);

            if (countryResponseFromList == null)
                return null;

            return countryResponseFromList.ToCountryResponse();
        }
    }
}