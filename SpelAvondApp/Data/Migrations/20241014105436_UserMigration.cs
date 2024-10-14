using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelAvondApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GeenAlcohol",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HeeftLactoseAllergie",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HeeftNotenAllergie",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVegetarisch",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeenAlcohol",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HeeftLactoseAllergie",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HeeftNotenAllergie",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsVegetarisch",
                table: "AspNetUsers");
        }
    }
}
