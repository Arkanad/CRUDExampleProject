using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"CREATE PROCEDURE [dbo].[InsertPerson]
(@PersonName varchar(40), @PersonId uniqueidentifier, @Email varchar(50), @Address varchar(1000), @DateOfBirth datetime2(7), @Gender varchar(10), @CountryId uniqueidentifier, @ReceiveNewsLetters bit) 
AS BEGIN
INSERT INTO [dbo].[Persons](PersonName, PersonId, Email, Address, DateOfBirth, Gender, CountryId, ReceiveNewsLetters) VALUES (@PersonName, @PersonId, @Email, @Address, @DateOfBirth,@Gender, @CountryId, @ReceiveNewsLetters)
 END";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"DROP PROCEDURE [dbo].[InsertPerson]";
            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}
