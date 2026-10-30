using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InformacioniSistemOnlineUcenje.Migrations
{
    /// <inheritdoc />
    public partial class IdentitySetupFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lekcije_Kursevi_KursID",
                table: "Lekcije");

            migrationBuilder.AlterColumn<string>(
                name: "Naziv",
                table: "Lekcije",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_LekcijaStatusi_KorisnikID",
                table: "LekcijaStatusi",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_LekcijaStatusi_LekcijaID",
                table: "LekcijaStatusi",
                column: "LekcijaID");

            migrationBuilder.AddForeignKey(
                name: "FK_LekcijaStatusi_Korisnici_KorisnikID",
                table: "LekcijaStatusi",
                column: "KorisnikID",
                principalTable: "Korisnici",
                principalColumn: "KorisnikID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LekcijaStatusi_Lekcije_LekcijaID",
                table: "LekcijaStatusi",
                column: "LekcijaID",
                principalTable: "Lekcije",
                principalColumn: "LekcijaID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lekcije_Kursevi_KursID",
                table: "Lekcije",
                column: "KursID",
                principalTable: "Kursevi",
                principalColumn: "KursID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LekcijaStatusi_Korisnici_KorisnikID",
                table: "LekcijaStatusi");

            migrationBuilder.DropForeignKey(
                name: "FK_LekcijaStatusi_Lekcije_LekcijaID",
                table: "LekcijaStatusi");

            migrationBuilder.DropForeignKey(
                name: "FK_Lekcije_Kursevi_KursID",
                table: "Lekcije");

            migrationBuilder.DropIndex(
                name: "IX_LekcijaStatusi_KorisnikID",
                table: "LekcijaStatusi");

            migrationBuilder.DropIndex(
                name: "IX_LekcijaStatusi_LekcijaID",
                table: "LekcijaStatusi");

            migrationBuilder.AlterColumn<string>(
                name: "Naziv",
                table: "Lekcije",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddForeignKey(
                name: "FK_Lekcije_Kursevi_KursID",
                table: "Lekcije",
                column: "KursID",
                principalTable: "Kursevi",
                principalColumn: "KursID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
