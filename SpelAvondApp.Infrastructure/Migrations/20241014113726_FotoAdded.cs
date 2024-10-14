using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelAvondApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FotoAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoPath",
                table: "Bordspellen",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoPath",
                table: "Bordspellen");
        }
    }
}
