using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelAvondApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInschrijvingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inschrijvingen_IdentityUser_SpelerId",
                table: "Inschrijvingen");

            migrationBuilder.DropIndex(
                name: "IX_Inschrijvingen_SpelerId",
                table: "Inschrijvingen");

            migrationBuilder.AlterColumn<string>(
                name: "SpelerId",
                table: "Inschrijvingen",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SpelerId",
                table: "Inschrijvingen",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Inschrijvingen_SpelerId",
                table: "Inschrijvingen",
                column: "SpelerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inschrijvingen_IdentityUser_SpelerId",
                table: "Inschrijvingen",
                column: "SpelerId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
