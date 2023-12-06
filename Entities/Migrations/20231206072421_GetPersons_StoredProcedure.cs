using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetPersons_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"CREATE PROCEDURE [dbo].[GetAllPersons]
            AS BEGIN
SELECT PersonId, PersonName, Address, ReceiveNewsLetters, Gender, CountryId FROM [dbo].[Persons]
END
";
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"DROP PROCEDURE [dbo].[GetAllPersons]
            AS BEGIN
SELECT PersonId, PersonName, Address, ReceiveNewsLetters, Gender, CountryId FROM [dbo].[Persons]
END
";
        }
    }
    }
}
