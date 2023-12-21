﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class TIN_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TIN",
                table: "Persons",
                newName: "TaxIdentificationNumber");

            migrationBuilder.AlterColumn<string>(
                name: "TaxIdentificationNumber",
                table: "Persons",
                type: "varchar(10)",
                nullable: true,
                defaultValue: "ABC1234567",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("012107df-862f-4f16-ba94-e5c16886f005"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("19f24c60-a4b3-41c6-b013-0a318aec8f35"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("19fce3c7-24e2-4e0a-a597-449e9bc3b21c"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("29339209-63f5-492f-8459-754943c74abf"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("2fd7baa0-f14b-4462-a85b-bb074d30d8ad"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("d92f1564-4a05-46e5-bab3-cc23286e287b"),
                column: "TaxIdentificationNumber",
                value: "ABC1234567");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxIdentificationNumber",
                table: "Persons",
                newName: "TIN");

            migrationBuilder.AlterColumn<string>(
                name: "TIN",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldNullable: true,
                oldDefaultValue: "ABC1234567");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("012107df-862f-4f16-ba94-e5c16886f005"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("19f24c60-a4b3-41c6-b013-0a318aec8f35"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("19fce3c7-24e2-4e0a-a597-449e9bc3b21c"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("29339209-63f5-492f-8459-754943c74abf"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("2fd7baa0-f14b-4462-a85b-bb074d30d8ad"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("d92f1564-4a05-46e5-bab3-cc23286e287b"),
                column: "TIN",
                value: null);
        }
    }
}