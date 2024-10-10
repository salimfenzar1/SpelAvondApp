using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelAvondApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBordspellenAvondStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BordspellenAvonden_IdentityUser_OrganisatorId",
                table: "BordspellenAvonden");

            migrationBuilder.DropTable(
                name: "BordspellenAvondBordspellen");

            migrationBuilder.AlterColumn<string>(
                name: "OrganisatorId",
                table: "BordspellenAvonden",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "BordspelId",
                table: "BordspellenAvonden",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BordspelId1",
                table: "BordspellenAvonden",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BordspellenAvonden_BordspelId",
                table: "BordspellenAvonden",
                column: "BordspelId");

            migrationBuilder.CreateIndex(
                name: "IX_BordspellenAvonden_BordspelId1",
                table: "BordspellenAvonden",
                column: "BordspelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BordspellenAvonden_Bordspellen_BordspelId",
                table: "BordspellenAvonden",
                column: "BordspelId",
                principalTable: "Bordspellen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BordspellenAvonden_Bordspellen_BordspelId1",
                table: "BordspellenAvonden",
                column: "BordspelId1",
                principalTable: "Bordspellen",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BordspellenAvonden_IdentityUser_OrganisatorId",
                table: "BordspellenAvonden",
                column: "OrganisatorId",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BordspellenAvonden_Bordspellen_BordspelId",
                table: "BordspellenAvonden");

            migrationBuilder.DropForeignKey(
                name: "FK_BordspellenAvonden_Bordspellen_BordspelId1",
                table: "BordspellenAvonden");

            migrationBuilder.DropForeignKey(
                name: "FK_BordspellenAvonden_IdentityUser_OrganisatorId",
                table: "BordspellenAvonden");

            migrationBuilder.DropIndex(
                name: "IX_BordspellenAvonden_BordspelId",
                table: "BordspellenAvonden");

            migrationBuilder.DropIndex(
                name: "IX_BordspellenAvonden_BordspelId1",
                table: "BordspellenAvonden");

            migrationBuilder.DropColumn(
                name: "BordspelId",
                table: "BordspellenAvonden");

            migrationBuilder.DropColumn(
                name: "BordspelId1",
                table: "BordspellenAvonden");

            migrationBuilder.AlterColumn<string>(
                name: "OrganisatorId",
                table: "BordspellenAvonden",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BordspellenAvondBordspellen",
                columns: table => new
                {
                    BordspellenAvondenId = table.Column<int>(type: "int", nullable: false),
                    BordspellenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BordspellenAvondBordspellen", x => new { x.BordspellenAvondenId, x.BordspellenId });
                    table.ForeignKey(
                        name: "FK_BordspellenAvondBordspellen_BordspellenAvonden_BordspellenAvondenId",
                        column: x => x.BordspellenAvondenId,
                        principalTable: "BordspellenAvonden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BordspellenAvondBordspellen_Bordspellen_BordspellenId",
                        column: x => x.BordspellenId,
                        principalTable: "Bordspellen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BordspellenAvondBordspellen_BordspellenId",
                table: "BordspellenAvondBordspellen",
                column: "BordspellenId");

            migrationBuilder.AddForeignKey(
                name: "FK_BordspellenAvonden_IdentityUser_OrganisatorId",
                table: "BordspellenAvonden",
                column: "OrganisatorId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
