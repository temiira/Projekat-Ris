using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InformacioniSistemOnlineUcenje.Migrations
{
    /// <inheritdoc />
    public partial class PopraviKoloneLekcije : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kursevi_Korisnici_NastavnikID",
                table: "Kursevi");

            migrationBuilder.DropForeignKey(
                name: "FK_LekcijaStatusi_Korisnici_KorisnikID",
                table: "LekcijaStatusi");

            migrationBuilder.DropForeignKey(
                name: "FK_LekcijaStatusi_Lekcije_LekcijaID",
                table: "LekcijaStatusi");

            migrationBuilder.DropForeignKey(
                name: "FK_Lekcije_Kursevi_KursID",
                table: "Lekcije");

            migrationBuilder.DropForeignKey(
                name: "FK_Napredak_Korisnici_KorisnikID",
                table: "Napredak");

            migrationBuilder.DropForeignKey(
                name: "FK_Napredak_Kursevi_KursID",
                table: "Napredak");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Kursevi_KursID",
                table: "Screenshots");

            migrationBuilder.DropForeignKey(
                name: "FK_Upisi_Korisnici_KorisnikID",
                table: "Upisi");

            migrationBuilder.DropForeignKey(
                name: "FK_Upisi_Kursevi_KursID",
                table: "Upisi");

            migrationBuilder.DropIndex(
                name: "IX_LekcijaStatusi_KorisnikID",
                table: "LekcijaStatusi");

            migrationBuilder.DropIndex(
                name: "IX_LekcijaStatusi_LekcijaID",
                table: "LekcijaStatusi");

            migrationBuilder.RenameColumn(
                name: "ProcitanoVrijeme",
                table: "LekcijaStatusi",
                newName: "DatumZavrsetka");

            migrationBuilder.RenameColumn(
                name: "StatusID",
                table: "LekcijaStatusi",
                newName: "ID");

            migrationBuilder.AlterColumn<int>(
                name: "KursID",
                table: "Screenshots",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "SlikaBajtovi",
                table: "Screenshots",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "Sadrzaj",
                table: "Lekcije",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Naziv",
                table: "Lekcije",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumDodavanja",
                table: "Lekcije",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikID",
                table: "LekcijaStatusi",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcitano",
                table: "LekcijaStatusi",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Opis",
                table: "Kursevi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Naziv",
                table: "Kursevi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "NastavnikID",
                table: "Kursevi",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Uloga",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Prezime",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Lozinka",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Ime",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumRegistracije",
                table: "Korisnici",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Kursevi_Korisnici_NastavnikID",
                table: "Kursevi",
                column: "NastavnikID",
                principalTable: "Korisnici",
                principalColumn: "KorisnikID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lekcije_Kursevi_KursID",
                table: "Lekcije",
                column: "KursID",
                principalTable: "Kursevi",
                principalColumn: "KursID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Napredak_Korisnici_KorisnikID",
                table: "Napredak",
                column: "KorisnikID",
                principalTable: "Korisnici",
                principalColumn: "KorisnikID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Napredak_Kursevi_KursID",
                table: "Napredak",
                column: "KursID",
                principalTable: "Kursevi",
                principalColumn: "KursID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Screenshots_Kursevi_KursID",
                table: "Screenshots",
                column: "KursID",
                principalTable: "Kursevi",
                principalColumn: "KursID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Upisi_Korisnici_KorisnikID",
                table: "Upisi",
                column: "KorisnikID",
                principalTable: "Korisnici",
                principalColumn: "KorisnikID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Upisi_Kursevi_KursID",
                table: "Upisi",
                column: "KursID",
                principalTable: "Kursevi",
                principalColumn: "KursID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kursevi_Korisnici_NastavnikID",
                table: "Kursevi");

            migrationBuilder.DropForeignKey(
                name: "FK_Lekcije_Kursevi_KursID",
                table: "Lekcije");

            migrationBuilder.DropForeignKey(
                name: "FK_Napredak_Korisnici_KorisnikID",
                table: "Napredak");

            migrationBuilder.DropForeignKey(
                name: "FK_Napredak_Kursevi_KursID",
                table: "Napredak");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Kursevi_KursID",
                table: "Screenshots");

            migrationBuilder.DropForeignKey(
                name: "FK_Upisi_Korisnici_KorisnikID",
                table: "Upisi");

            migrationBuilder.DropForeignKey(
                name: "FK_Upisi_Kursevi_KursID",
                table: "Upisi");

            migrationBuilder.DropColumn(
                name: "SlikaBajtovi",
                table: "Screenshots");

            migrationBuilder.DropColumn(
                name: "IsProcitano",
                table: "LekcijaStatusi");

            migrationBuilder.RenameColumn(
                name: "DatumZavrsetka",
                table: "LekcijaStatusi",
                newName: "ProcitanoVrijeme");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "LekcijaStatusi",
                newName: "StatusID");

            migrationBuilder.AlterColumn<int>(
                name: "KursID",
                table: "Screenshots",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Sadrzaj",
                table: "Lekcije",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Naziv",
                table: "Lekcije",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumDodavanja",
                table: "Lekcije",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KorisnikID",
                table: "LekcijaStatusi",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Opis",
                table: "Kursevi",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Naziv",
                table: "Kursevi",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NastavnikID",
                table: "Kursevi",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Uloga",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Prezime",
                table: "Korisnici",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Lozinka",
                table: "Korisnici",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ime",
                table: "Korisnici",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumRegistracije",
                table: "Korisnici",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LekcijaStatusi_KorisnikID",
                table: "LekcijaStatusi",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_LekcijaStatusi_LekcijaID",
                table: "LekcijaStatusi",
                column: "LekcijaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Kursevi_Korisnici_NastavnikID",
                table: "Kursevi",
                column: "NastavnikID",
                principalTable: "Korisnici",
                principalColumn: "KorisnikID",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Napredak_Korisnici_KorisnikID",
                table: "Napredak",
                column: "KorisnikID",
                principalTable: "Korisnici",
                principalColumn: "KorisnikID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Napredak_Kursevi_KursID",
                table: "Napredak",
                column: "KursID",
                principalTable: "Kursevi",
                principalColumn: "KursID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Screenshots_Kursevi_KursID",
                table: "Screenshots",
                column: "KursID",
                principalTable: "Kursevi",
                principalColumn: "KursID");

            migrationBuilder.AddForeignKey(
                name: "FK_Upisi_Korisnici_KorisnikID",
                table: "Upisi",
                column: "KorisnikID",
                principalTable: "Korisnici",
                principalColumn: "KorisnikID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Upisi_Kursevi_KursID",
                table: "Upisi",
                column: "KursID",
                principalTable: "Kursevi",
                principalColumn: "KursID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
