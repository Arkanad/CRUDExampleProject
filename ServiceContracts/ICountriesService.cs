﻿using ServiceContracts.DTO;

namespace ServiceContracts
{

    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">Country object to add</param>
        /// <returns>Returns the country object after adding it (including newly generated country id)</returns>
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns all the countries
        /// </summary>
        /// <returns>All countries from the list as List of CountryResponse</CountryResponse></returns>
        List<CountryResponse> GetAllCountries();

        /// <summary>
        /// Return country  object based on country id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>matching country as CountryResponse object</returns>
        CountryResponse GetCountryByCountryId(Guid? guid);
    }
}