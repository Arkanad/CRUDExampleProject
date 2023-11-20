using System.ComponentModel;
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
        private readonly ICountriesService _countriesService;


        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
            if (initialize)
            {
                _persons.Add(new Person()
                {
                    PersonName = "Zelig",
                    Email = "zasty0@wiley.com",
                    DateOfBirth = DateTime.Parse("27-08-1990"),
                    Gender = "Male",
                    Address = "68822 Dennis Avenue",
                    ReceiveNewLetter = true,
                    PersonId = Guid.Parse("F236BC89-5437-40A8-A2FC-A68A0FA11E02"),
                    CountryId = Guid.Parse("E4B5AEB7-B2AE-492A-9233-BF4EB12F7F0C")
                });

                _persons.Add(new Person()
                {
                    PersonName = "Humphrey",
                    Email = "hwinsley1@ifeng.com",
                    DateOfBirth = DateTime.Parse("03-03-2000"),
                    Gender = "Male",
                    Address = "47 Troy Circle",
                    ReceiveNewLetter = false,
                    PersonId = Guid.Parse("6375C839-4639-4E56-A2F0-CC603B461DFF"),
                    CountryId = Guid.Parse("74EA22F5-3271-47A2-893E-B4C464CA9B9C")
                });
                _persons.Add(new Person()
                {
                    PersonName = "Beverlee",
                    Email = "bounsworth2@sphinn.com",
                    DateOfBirth = DateTime.Parse("27-08-1990"),
                    Gender = "Female",
                    Address = "3152 Grim Crossing",
                    ReceiveNewLetter = true,
                    PersonId = Guid.Parse("5232BD3E-472E-4E51-9749-C3D144BC6A76"),
                    CountryId = Guid.Parse("1C63B612-62C4-4DD4-BA5A-EBFAA9847CC1")
                });
                _persons.Add(new Person()
                {
                    PersonName = "Chelsey",
                    Email = "calbon3@theglobeandmail.com",
                    DateOfBirth = DateTime.Parse("17-09-1992"),
                    Gender = "Female",
                    Address = "68822 Dennis Avenue",
                    ReceiveNewLetter = true,
                    PersonId = Guid.Parse("B919F8A1-56F7-4147-9EFB-4C0E21B73843"),
                    CountryId = Guid.Parse("E4B5AEB7-B2AE-492A-9233-BF4EB12F7F0C")
                });
                _persons.Add(
                    new Person()
                    {
                        PersonName = "Emmalee",
                        Email = "enimmo4@webeden.co.uk",
                        DateOfBirth = DateTime.Parse("19-03-1998"),
                        Gender = "Female",
                        Address = "75 Old Gate Road",
                        ReceiveNewLetter = false,
                        PersonId = Guid.Parse("E988E2DA-0021-45EB-988C-D8BB0DBFA12C"),
                        CountryId = Guid.Parse("66EBD433-B673-4790-832D-D9500A831D12")
                    });
                _persons.Add(new Person()
                {
                    PersonName = "Linzy",
                    Email = "ljack5@nationalgeographic.com",
                    DateOfBirth = DateTime.Parse("27-07-1993"),
                    Gender = "Female",
                    Address = "4 Larry Court",
                    ReceiveNewLetter = true,
                    PersonId = Guid.Parse("EB0BD505-34E4-4AB7-A9B1-13064ADAE9F2"),
                    CountryId = Guid.Parse("E4B5AEB7-B2AE-492A-9233-BF4EB12F7F0C")
                });
            }

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
                CountryId = Guid.NewGuid(),
                Gender = personAddRequest.Gender.ToString()
            };

            _persons.Add(person);
            return ConvertPersonToPersonResponse(person);
        }

        

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
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
