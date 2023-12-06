using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder) //Этот метод вызывается, если модель для производного контекста была инициализирована
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seed to Countries
            string _countries = File.ReadAllText("countries.json");
            List<Country> countries = JsonSerializer.Deserialize<List<Country>>(_countries);
            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            //Seed to Persons
            string _persons = File.ReadAllText("persons.json");
            List<Person> persons = JsonSerializer.Deserialize<List<Person>>(_persons);
            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }
        }

        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }
    }
}
