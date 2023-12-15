using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person Name is required")]
        [MinLength(3,ErrorMessage = "Name length should have at least 3 characters")]
        public string? PersonName { get; set; }
        [Required]
        [EmailAddress (ErrorMessage = "Email should be in valid format")]
        [DataType(DataType.DateTime)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public GenderOptions? Gender { get; set; }
        
        public string? Address { get; set; }
        
        public bool ReceiveNewsLetters { get; set; }

        [Required]
        public Guid? CountryId { get; set; }

        /// <summary>
        /// Converts the current object of PersonAddRequest into a new object of Person type
        /// </summary>
        /// <returns>Person object from PersonAddRequest</returns>
        public Person ToPerson()
        {
            return new Person()
            {
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
