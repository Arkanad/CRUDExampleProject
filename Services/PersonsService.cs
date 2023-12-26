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
        private readonly PersonsDbContext db;
        private readonly ICountriesService _countriesService;
       
        public PersonsService(PersonsDbContext personDBContext, ICountriesService countriesService)
        {
            db = personDBContext;
            _countriesService = countriesService;
        }
       
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = new Person() {
                PersonName = personAddRequest.PersonName,
                PersonId = Guid.NewGuid(),
                Email = personAddRequest.Email,
                Address = personAddRequest.Address,
                DateOfBirth = personAddRequest.DateOfBirth,
                CountryId = personAddRequest.CountryId,
                Gender = personAddRequest.Gender.ToString()
            };

            db.Persons.Add(person);
            await db.SaveChangesAsync();
            //db.sp_InsertPerson(person);
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var persons = await db.Persons.Include("Country").ToListAsync();

            return db.Persons.ToList().Select(person => person.ToPersonResponse()).ToList();
            //return db.sp_GetAllPersons().Select(temp => ConvertPersonToPersonResponse(temp)).ToList();
        }

        public async Task<PersonResponse?> GetPersonById(Guid? personID)
        {
            if (personID == null)
                return null;

            Person? person = await db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personID);

            if (person == null)
                return null;

            return person.ToPersonResponse();
        }
        
        public async Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = await GetAllPersons();
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

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string? sortBy, SortOrderOptions sortOrder)
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

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
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

            await db.SaveChangesAsync();
            return matchingPersonToUpdate.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? personId)
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
            await db.SaveChangesAsync();

            return true;
        }

        //public string GenerateTIN()
        //{
        //    int length = 10;
        //    var random = new Random();

        //    string s = string.Empty;
        //    for (int i = 0; i < length; i++)
        //        s = String.Concat(s, random.Next(10).ToString());
        //    return s;
        //}
    }
}
