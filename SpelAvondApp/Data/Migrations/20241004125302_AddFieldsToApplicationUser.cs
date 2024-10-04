using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelAvondApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Geboortedatum",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Geslacht",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Huisnummer",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Naam",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Stad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Straat",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Geboortedatum",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Geslacht",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Huisnummer",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Naam",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Stad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Straat",
                table: "AspNetUsers");
        }
    }
}
