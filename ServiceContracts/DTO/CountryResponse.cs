using System;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type for most of CountryService methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryId { get; set; }

        public string? CountryName { get; set; }

        //It compares the current object to another object of CountryResponse type and returns true if both values are same; otherwise returns false
        public override bool Equals(object? obj)
        {
            if(obj == null)
            {
                return false;
            }
            if(obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }
            CountryResponse countryToCompare = (CountryResponse)obj;

            return this.CountryName == countryToCompare.CountryName && this.CountryId == countryToCompare.CountryId;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };
        }
    }
}
