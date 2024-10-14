using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelAvondApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SpellenMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BiedtAlcoholvrijeOpties",
                table: "BordspellenAvonden",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BiedtLactosevrijeOpties",
                table: "BordspellenAvonden",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BiedtNotenvrijeOpties",
                table: "BordspellenAvonden",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BiedtVegetarischeOpties",
                table: "BordspellenAvonden",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiedtAlcoholvrijeOpties",
                table: "BordspellenAvonden");

            migrationBuilder.DropColumn(
                name: "BiedtLactosevrijeOpties",
                table: "BordspellenAvonden");

            migrationBuilder.DropColumn(
                name: "BiedtNotenvrijeOpties",
                table: "BordspellenAvonden");

            migrationBuilder.DropColumn(
                name: "BiedtVegetarischeOpties",
                table: "BordspellenAvonden");
        }
    }
}
