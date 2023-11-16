﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required]
        public string? PersonName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
       
        public GenderOptions? Gender { get; set; }
        
        public string? Address { get; set; }
        
        public bool ReceiveNewsLetters { get; set; }
       
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
                ReceiveNewLetter = ReceiveNewsLetters,
                CountryId = CountryId
            };
        }
    }
}