using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelAvondApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VerwijderOrganisatorForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BordspellenAvonden_IdentityUser_OrganisatorId",
                table: "BordspellenAvonden");

            migrationBuilder.DropIndex(
                name: "IX_BordspellenAvonden_OrganisatorId",
                table: "BordspellenAvonden");

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

            migrationBuilder.AlterColumn<string>(
                name: "OrganisatorId",
                table: "BordspellenAvonden",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "OrganisatorId",
                table: "BordspellenAvonden",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BordspellenAvonden_OrganisatorId",
                table: "BordspellenAvonden",
                column: "OrganisatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BordspellenAvonden_IdentityUser_OrganisatorId",
                table: "BordspellenAvonden",
                column: "OrganisatorId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
