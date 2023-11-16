using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Linq;
using Services.Helpers;
using ServiceContracts.Enums;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;

        public PersonsService()
        {
            _persons = new List<Person>();
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
                CountryId = Guid.NewGuid(),
                Gender = personAddRequest.Gender.ToString()
            };

            _persons.Add(person);
            return person.ToPersonResponse();
        }
        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(person => person.ToPersonResponse()).ToList();
        }

        public PersonResponse GetPersonById(Guid? personID)
        {
            if (personID == null)
                return null;

            Person? person = _persons.FirstOrDefault(temp => temp.PersonId == personID);

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
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(temp =>
                        (!string.IsNullOrEmpty(temp.PersonName)
                            ? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true)).ToList();
                        break;

                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(temp =>
                        (!string.IsNullOrEmpty(temp.Email)
                            ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true)).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(temp =>
                        (temp.DateOfBirth != null ? temp.DateOfBirth.Value.ToString("yy-MM-dd").Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true)).ToList();
                    break;

                case nameof(Person.CountryId):
                    matchingPersons = allPersons.Where(temp =>
                        (string.IsNullOrEmpty(temp.CountryName)
                            ? temp.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true)).ToList();
                    break;

                case nameof(Person.Address):
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
                (nameof(PersonResponse.CountryId), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.CountryId).ToList(),
                (nameof(PersonResponse.CountryId), SortOrderOptions.Desc)
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
                (nameof(PersonResponse.CountryName), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.CountryName), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.ReceiveNewLetter), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.ReceiveNewLetter).ToList(),
                (nameof(PersonResponse.ReceiveNewLetter), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.ReceiveNewLetter).ToList(),
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
            Person? matchingPersonToUpdate = _persons.FirstOrDefault(temp => temp.PersonId == personUpdateRequest.PersonId);
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
            matchingPersonToUpdate.ReceiveNewLetter = personUpdateRequest.ReceiveNewsLetters;
            matchingPersonToUpdate.CountryId = personUpdateRequest.CountryId;

            return matchingPersonToUpdate.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //get matching person object to update
            Person? matchingPersonToDelete = _persons.FirstOrDefault(temp => temp.PersonId == personId);
            if (matchingPersonToDelete == null)
            {
                return false;
            }

            _persons.RemoveAll(person => person.PersonId == personId);

            return true;
        }
    }
}
