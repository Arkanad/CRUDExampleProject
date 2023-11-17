using ServiceContracts;
using ServiceContracts.DTO;
using System;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(false);
        }

        #region AddCountry

        [Fact]
        //when CountryAddRequest is null, it should ArgumentNullException
        public void AddCountryRequest_IsNull()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        [Fact]
        //when the CountryName is null, it should throw ArgumentException 
        public void AddCountryRequest_CountryIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            if (request.CountryName == null)
            {
                //Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    //act
                    _countriesService.AddCountry(request);
                });
            }
        }

        [Fact]
        //When the CountryName is duplicate, it should throw ArgumentException
        public void AddCountryRequest_DuplicateCountryName()
        {

            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        [Fact]
        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        public void AddCountryRequest_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest request1 = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            CountryResponse response = _countriesService.AddCountry(request1);
            List<CountryResponse> countriesFromGetAllCountries = _countriesService.GetAllCountries();

            Assert.True(response.CountryId != Guid.Empty);
            Assert.Contains(response, countriesFromGetAllCountries);
        }
        #endregion

        #region GetAllCountries

        [Fact]
        //The list should be empty by default (before adding any countries)
        public void GetAllCountries_EmptyList()
        {
            //Acts
            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actualCountryResponseList);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> countryRequestList = new List<CountryAddRequest>()
            {
                new CountryAddRequest(){CountryName = "Ukraine"},
                new CountryAddRequest(){CountryName = "Czechia"}
            };

            //Act

            List<CountryResponse> expectedCountriesListFromAddCountry = new List<CountryResponse>();
            foreach (CountryAddRequest countryRequest in countryRequestList)
            {
                expectedCountriesListFromAddCountry.Add(_countriesService.AddCountry(countryRequest));
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            foreach (CountryResponse expectedCountry in expectedCountriesListFromAddCountry)
            {
                Assert.Contains(expectedCountry, actualCountryResponseList);
            }
        }

        #endregion

        #region GetCountyByCountryID

        [Fact]
        //If we supply null as CountryId, it should return null as CountryResponse
        public void GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? guid = null;

            //Act
            CountryResponse countryResponseFromGetMethod = _countriesService.GetCountryByCountryId(guid);

            //Assert
            Assert.Null(countryResponseFromGetMethod);
        }

         [Fact]
        //If we supply a valid country ID, it should return the matching country details as CountryResponse
        public void GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "China" };
           CountryResponse countryResponseFromAdd = _countriesService.AddCountry(countryAddRequest);

            //Act
           CountryResponse countryResponseFromGet = _countriesService.GetCountryByCountryId(countryResponseFromAdd.CountryId);

            //Assert
            Assert.Equal(countryResponseFromAdd, countryResponseFromGet);
        }
        #endregion
    }
}