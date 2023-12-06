using Entities;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// adds a new person in to the list of persons
        /// </summary>
        /// <param name="personAddRequest">person to add</param>
        /// <returns>Returns the same person details, along with newly generated PersonID</returns>
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        public List<PersonResponse> GetAllPersons();

        /// <summary>
        /// Returns person object based on personID
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>matching person as PersonResponse object</returns>
        public PersonResponse GetPersonById(Guid? personId);

        /// <summary>
        /// Return all person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search field to search</param>
        /// <returns>Returns all matching persons based on the given search field and search string</returns>
        public List<PersonResponse> GetFilteredPersons(string? searchBy, string? searchString);

        /// <summary>
        /// Returns sorted list of persons
        /// </summary>
        /// <param name="allPersons">represent list of persons to sort</param>
        /// <param name="sortBy">Name of the property(key), based on which the persons should be sorted</param>
        /// <param name="sortOrder">Descending or Ascending </param>
        /// <returns>Returns sorted persons as PersonResponse list</returns>
        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string? sortBy,
            SortOrderOptions sortOrder);

        /// <summary>
        /// Update specified person details based on the given person ID
        /// </summary>
        /// <param name="personUpdateRequest">Person detail to update, including person id</param>
        /// <returns>Return PersonResponse object after updation</returns>
        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Delete specified person based on the given person ID
        /// </summary>
        /// <param name="personId">person`s id to delete person</param>
        /// <returns>Return bool object providing result of deleting object</returns>
        public bool DeletePerson(Guid? personId);

    }
}
