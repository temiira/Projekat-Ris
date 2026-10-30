using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InformacioniSistemOnlineUcenje.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    KorisnikID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lozinka = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Uloga = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SlikaBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumRegistracije = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.KorisnikID);
                });

            migrationBuilder.CreateTable(
                name: "LekcijaStatusi",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikID = table.Column<int>(type: "int", nullable: false),
                    LekcijaID = table.Column<int>(type: "int", nullable: false),
                    ProcitanoVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LekcijaStatusi", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Kursevi",
                columns: table => new
                {
                    KursID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NastavnikID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kursevi", x => x.KursID);
                    table.ForeignKey(
                        name: "FK_Kursevi_Korisnici_NastavnikID",
                        column: x => x.NastavnikID,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lekcije",
                columns: table => new
                {
                    LekcijaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KursID = table.Column<int>(type: "int", nullable: false),
                    SlikaBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumDodavanja = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lekcije", x => x.LekcijaID);
                    table.ForeignKey(
                        name: "FK_Lekcije_Kursevi_KursID",
                        column: x => x.KursID,
                        principalTable: "Kursevi",
                        principalColumn: "KursID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Napredak",
                columns: table => new
                {
                    NapredakID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikID = table.Column<int>(type: "int", nullable: false),
                    KursID = table.Column<int>(type: "int", nullable: false),
                    Procenat = table.Column<int>(type: "int", nullable: false),
                    DatumAzuriranja = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Napredak", x => x.NapredakID);
                    table.ForeignKey(
                        name: "FK_Napredak_Korisnici_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Napredak_Kursevi_KursID",
                        column: x => x.KursID,
                        principalTable: "Kursevi",
                        principalColumn: "KursID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Screenshots",
                columns: table => new
                {
                    ScreenshotID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KursID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenshots", x => x.ScreenshotID);
                    table.ForeignKey(
                        name: "FK_Screenshots_Kursevi_KursID",
                        column: x => x.KursID,
                        principalTable: "Kursevi",
                        principalColumn: "KursID");
                });

            migrationBuilder.CreateTable(
                name: "Upisi",
                columns: table => new
                {
                    UpisID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikID = table.Column<int>(type: "int", nullable: false),
                    KursID = table.Column<int>(type: "int", nullable: false),
                    DatumUpisa = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upisi", x => x.UpisID);
                    table.ForeignKey(
                        name: "FK_Upisi_Korisnici_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Upisi_Kursevi_KursID",
                        column: x => x.KursID,
                        principalTable: "Kursevi",
                        principalColumn: "KursID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kursevi_NastavnikID",
                table: "Kursevi",
                column: "NastavnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Lekcije_KursID",
                table: "Lekcije",
                column: "KursID");

            migrationBuilder.CreateIndex(
                name: "IX_Napredak_KorisnikID",
                table: "Napredak",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Napredak_KursID",
                table: "Napredak",
                column: "KursID");

            migrationBuilder.CreateIndex(
                name: "IX_Screenshots_KursID",
                table: "Screenshots",
                column: "KursID");

            migrationBuilder.CreateIndex(
                name: "IX_Upisi_KorisnikID",
                table: "Upisi",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Upisi_KursID",
                table: "Upisi",
                column: "KursID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LekcijaStatusi");

            migrationBuilder.DropTable(
                name: "Lekcije");

            migrationBuilder.DropTable(
                name: "Napredak");

            migrationBuilder.DropTable(
                name: "Screenshots");

            migrationBuilder.DropTable(
                name: "Upisi");

            migrationBuilder.DropTable(
                name: "Kursevi");

            migrationBuilder.DropTable(
                name: "Korisnici");
        }
    }
}
