using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using ServiceContracts.Enums;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly PersonDbContext db;
        private readonly ICountriesService _countriesService;
       
        public PersonsService(PersonDbContext personDBContext, ICountriesService countriesService)
        {
            db = personDBContext;
            _countriesService = countriesService;
        }
    

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.CountryName = _countriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = new Person(){
                PersonName = personAddRequest.PersonName,
                PersonId = Guid.NewGuid(),
                Email = personAddRequest.Email,
                Address = personAddRequest.Address,
                DateOfBirth = personAddRequest.DateOfBirth,
                CountryId = personAddRequest.CountryId,
                Gender = personAddRequest.Gender.ToString()
            };

            //db.Persons.Add(person);
            //db.SaveChanges();
            db.sp_InsertPerson(person);
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            //return db.Persons.ToList().Select(person => ConvertPersonToPersonResponse(person)).ToList();
            return db.sp_GetAllPersons().Select(temp => ConvertPersonToPersonResponse(temp)).ToList();
        }

        public PersonResponse GetPersonById(Guid? personID)
        {
            if (personID == null)
                return null;

            Person? person = db.Persons.FirstOrDefault(temp => temp.PersonId == personID);

            if (person == null)
                return null;

            return person.ToPersonResponse();
        }
        
        public List<PersonResponse> GetFilteredPersons(string? searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = new List<PersonResponse>();

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return allPersons;
            }

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(temp =>
                        (!string.IsNullOrEmpty(temp.PersonName)
                            ? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true)).ToList();
                        break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(temp =>
                        (!string.IsNullOrEmpty(temp.Email)
                            ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true)).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(temp =>
                        (temp.DateOfBirth != null ? temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase): true)).ToList();
                    break;

                case nameof(PersonResponse.CountryId):
                    matchingPersons = allPersons.Where(temp =>
                        (string.IsNullOrEmpty(temp.CountryName)
                            ? temp.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true)).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(temp =>
                        (string.IsNullOrEmpty(temp.Address)
                            ? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true)).ToList();
                    break;
                default: matchingPersons = allPersons;
                    break;
            }

            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string? sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.CountryName), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.CountryId).ToList(),
                (nameof(PersonResponse.CountryName), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.CountryId).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.Asc) 
                    => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.Age).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.Gender).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.Gender).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),
                _ => allPersons
            };
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPersonToUpdate = db.Persons.FirstOrDefault(temp => temp.PersonId == personUpdateRequest.PersonId);
            if (matchingPersonToUpdate == null)
            {
                throw new ArgumentException("Given person id doesn't exist");
            }

            //update all details
            matchingPersonToUpdate.PersonName = personUpdateRequest.PersonName;
            matchingPersonToUpdate.PersonId = personUpdateRequest.PersonId;
            matchingPersonToUpdate.Address = personUpdateRequest.Address;
            matchingPersonToUpdate.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPersonToUpdate.Email = personUpdateRequest.Email;
            matchingPersonToUpdate.Gender = personUpdateRequest.Gender.ToString();
            matchingPersonToUpdate.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            matchingPersonToUpdate.CountryId = personUpdateRequest.CountryId;

            db.SaveChanges();
            return ConvertPersonToPersonResponse(matchingPersonToUpdate);
        }

        public bool DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //get matching person object to update
            Person? matchingPersonToDelete = db.Persons.FirstOrDefault(temp => temp.PersonId == personId);
            if (matchingPersonToDelete == null)
            {
                return false;
            }

            db.Persons.Remove(db.Persons.First(temp=> temp.PersonId == personId));
            db.SaveChanges();

            return true;
        }
    }
}
