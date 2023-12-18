using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"CREATE PROCEDURE [dbo].[InsertPerson]
(@PersonId uniqueidentifier, @PersonName varchar(40), @Email varchar(50),@DateOfBirth datetime2(7), @Gender varchar(10),  @CountryId uniqueidentifier,@Address varchar(1000), @ReceiveNewsLetters bit) 
AS BEGIN
INSERT INTO [dbo].[Persons](PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetters) VALUES (@PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address,  @ReceiveNewsLetters)
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
