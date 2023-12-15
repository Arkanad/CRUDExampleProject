using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents the DTO class that contains the person details to update
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "PersonID can`t be blank")]
        public Guid PersonId { get; set; }

        [Required(ErrorMessage = "PersonName can`t be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can`t be blank")]
        [EmailAddress(ErrorMessage = "Email value should be in a valid email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Date of Birth can`t be blank")]
        public DateTime? DateOfBirth { get; set; }

        public GenderOptions? Gender { get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }

        [Required(ErrorMessage = "CountryId can't be blank")]
        public Guid? CountryId { get; set; }

        /// <summary>
        /// Converts the current object of PersonAddRequest into a new object of Person type
        /// </summary>
        /// <returns>Person object from PersonAddRequest</returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters,
                CountryId = CountryId
            };
        }
    }
}
