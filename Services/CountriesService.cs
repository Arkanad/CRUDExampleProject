using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private field
        private readonly List<Country> _countries;

        //constructor
        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>(){
                new Country()
                    { CountryID = Guid.Parse("E4B5AEB7-B2AE-492A-9233-BF4EB12F7F0C"), CountryName ="Ukraine" },
                new Country()
                    { CountryID = Guid.Parse("74EA22F5-3271-47A2-893E-B4C464CA9B9C"), CountryName = "Czech Republic" },
                new Country()
                    { CountryID = Guid.Parse("1C63B612-62C4-4DD4-BA5A-EBFAA9847CC1"), CountryName = "Netherlands" },
                new Country()
                    { CountryID = Guid.Parse("61B3E18E-B449-4509-B6DC-41C7E543D71F"), CountryName = "USA" },
                new Country()
                    { CountryID = Guid.Parse("66EBD433-B673-4790-832D-D9500A831D12"), CountryName = "French" }
                });
                
                
            }
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation: countryAddRequest parameter can't be null
            ValidationHelper.ModelValidation(countryAddRequest);

            //Validation: CountryName can't be duplicate
            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountryId
            country.CountryID = Guid.NewGuid();

            //Add country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse GetCountryByCountryId(Guid? countryID)
        {
            if (countryID == null)
                return null;

            Country? countryResponseFromList =  _countries.FirstOrDefault(temp => temp.CountryID == countryID);

            if (countryResponseFromList == null)
                return null;

            return countryResponseFromList.ToCountryResponse();
        }
    }
}