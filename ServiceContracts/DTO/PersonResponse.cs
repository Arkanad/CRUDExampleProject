using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

    

        /// <summary>
        /// Compares the current object data with the parameter object
        /// </summary>
        /// <param name="obj">the PersonResponse Object to compare</param>
        /// <returns>True of false, indicating whether all person details are matched with the specific parameter object</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(PersonResponse))
                return false;

            PersonResponse person = (PersonResponse)obj;
            return PersonId == person.PersonId && PersonName == person.PersonName && Email == person.Email && DateOfBirth == person.DateOfBirth && Gender == person.Gender && CountryId == person.CountryId &&
                   CountryName == person.CountryName && Address == person.Address && ReceiveNewsLetters == person.ReceiveNewsLetters && Age == person.Age;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Person Name: {PersonName}, Person Id: {PersonId}, Email: {Email}, CountryId:{CountryId}, CountryName: {CountryName}, Gender: {Gender}, Address:{Address}, ReceiveNewsLetters:{ReceiveNewsLetters}, DateOfBirth: {DateOfBirth?.ToString("dd MMM yyyy")}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonName = PersonName,
                Email = Email,
                PersonId = PersonId,
                Address = Address,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender),
                ReceiveNewsLetters = ReceiveNewsLetters,
                CountryId = CountryId
            };

        }
    }

    public static class PersonExtensions
    {
        /// <summary>
        /// An extension class to convert an object of Person class into PersonResponse
        /// </summary>
        /// <param name="person">the Person object to convert</param>
        /// <returns>Returns the converted PersonResponse object</returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonName = person.PersonName,
                PersonId = person.PersonId,
                Address = person.Address,
                DateOfBirth = person.DateOfBirth,
                Email = person.Email,
                ReceiveNewsLetters = person.ReceiveNewLetter,
                Gender = person.Gender,
                Age = (person.DateOfBirth != null)? Math.Round
                    ((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    }
}
