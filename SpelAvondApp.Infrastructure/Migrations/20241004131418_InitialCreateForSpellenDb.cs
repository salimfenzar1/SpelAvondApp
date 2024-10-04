using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelAvondApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateForSpellenDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bordspellen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Beschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is18Plus = table.Column<bool>(type: "bit", nullable: false),
                    SoortSpel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bordspellen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BordspellenAvonden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxAantalSpelers = table.Column<int>(type: "int", nullable: false),
                    Is18Plus = table.Column<bool>(type: "bit", nullable: false),
                    OrganisatorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BordspellenAvonden", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BordspellenAvonden_IdentityUser_OrganisatorId",
                        column: x => x.OrganisatorId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Inschrijvingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpelerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DieetWensen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BordspellenAvondId = table.Column<int>(type: "int", nullable: false),
                    Aanwezig = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inschrijvingen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inschrijvingen_BordspellenAvonden_BordspellenAvondId",
                        column: x => x.BordspellenAvondId,
                        principalTable: "BordspellenAvonden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inschrijvingen_IdentityUser_SpelerId",
                        column: x => x.SpelerId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BordspellenAvondId = table.Column<int>(type: "int", nullable: false),
                    SpelerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Opmerking = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_BordspellenAvonden_BordspellenAvondId",
                        column: x => x.BordspellenAvondId,
                        principalTable: "BordspellenAvonden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_IdentityUser_SpelerId",
                        column: x => x.SpelerId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BordspellenAvondBordspellen_BordspellenId",
                table: "BordspellenAvondBordspellen",
                column: "BordspellenId");

            migrationBuilder.CreateIndex(
                name: "IX_BordspellenAvonden_OrganisatorId",
                table: "BordspellenAvonden",
                column: "OrganisatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Inschrijvingen_BordspellenAvondId",
                table: "Inschrijvingen",
                column: "BordspellenAvondId");

            migrationBuilder.CreateIndex(
                name: "IX_Inschrijvingen_SpelerId",
                table: "Inschrijvingen",
                column: "SpelerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BordspellenAvondId",
                table: "Reviews",
                column: "BordspellenAvondId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_SpelerId",
                table: "Reviews",
                column: "SpelerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BordspellenAvondBordspellen");

            migrationBuilder.DropTable(
                name: "Inschrijvingen");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Bordspellen");

            migrationBuilder.DropTable(
                name: "BordspellenAvonden");

            migrationBuilder.DropTable(
                name: "IdentityUser");
        }
    }
}
