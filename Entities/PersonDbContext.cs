using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Entities
{
    public class PersonDbContext: DbContext 
    {
        public PersonDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //Цей метод викликається, якщо модель для похідного контексту була ініціалізована
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seed to Countries
            string _countries = File.ReadAllText("countries.json");
            List<Country>? countries = JsonSerializer.Deserialize<List<Country>>(_countries);
            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            //Seed to Persons
            string _persons = File.ReadAllText("persons.json");
            List<Person>? persons = JsonSerializer.Deserialize<List<Person>>(_persons);
            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }

            //fluent API
            //modelBuilder.Entity<Person>().Property(temp => temp.TIN);
        }

        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[] {
        new SqlParameter("@PersonId", person.PersonId),
        new SqlParameter("@PersonName", person.PersonName),
        new SqlParameter("@Email", person.Email),
        new SqlParameter("@DateOfBirth", person.DateOfBirth),
        new SqlParameter("@Gender", person.Gender),
        new SqlParameter("@CountryId", person.CountryId),
        new SqlParameter("@Address", person.Address),
        new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
      };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters", parameters);
            //@PersonName nvarchar(40), @PersonId uniqueidentifier, @Email nvarchar(50), @Address nvarchar(1000), @DateOfBirth datetime2(7), @Gender nvarchar(10), @CountryId uniqueidentifier, @ReceiveNewsLetters bit

        }
    }
}
