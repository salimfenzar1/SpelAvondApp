using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelAvondApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NaamVanDeMigratie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BordspellenAvonden_IdentityUser_OrganisatorId",
                table: "BordspellenAvonden");

            migrationBuilder.AddForeignKey(
                name: "FK_BordspellenAvonden_IdentityUser_OrganisatorId",
                table: "BordspellenAvonden",
                column: "OrganisatorId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BordspellenAvonden_IdentityUser_OrganisatorId",
                table: "BordspellenAvonden");

            migrationBuilder.AddForeignKey(
                name: "FK_BordspellenAvonden_IdentityUser_OrganisatorId",
                table: "BordspellenAvonden",
                column: "OrganisatorId",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }
    }
}
