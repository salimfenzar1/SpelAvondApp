using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelAvondApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inschrijvingen_IdentityUser_SpelerId",
                table: "Inschrijvingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_IdentityUser_SpelerId",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "IdentityUser",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Geboortedatum",
                table: "IdentityUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Geslacht",
                table: "IdentityUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Huisnummer",
                table: "IdentityUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Naam",
                table: "IdentityUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stad",
                table: "IdentityUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Straat",
                table: "IdentityUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inschrijvingen_IdentityUser_SpelerId",
                table: "Inschrijvingen",
                column: "SpelerId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_IdentityUser_SpelerId",
                table: "Reviews",
                column: "SpelerId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inschrijvingen_IdentityUser_SpelerId",
                table: "Inschrijvingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_IdentityUser_SpelerId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "Geboortedatum",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "Geslacht",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "Huisnummer",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "Naam",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "Stad",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "Straat",
                table: "IdentityUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Inschrijvingen_IdentityUser_SpelerId",
                table: "Inschrijvingen",
                column: "SpelerId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_IdentityUser_SpelerId",
                table: "Reviews",
                column: "SpelerId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
