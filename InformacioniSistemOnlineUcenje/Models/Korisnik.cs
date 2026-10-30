using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace InformacioniSistemOnlineUcenje.Models
{
    public class Korisnik
    {
        [Key]
        public int KorisnikID { get; set; }

        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Lozinka { get; set; } = string.Empty;
        public DateTime DatumRegistracije { get; set; } = DateTime.Now;
        public string Uloga { get; set; } = string.Empty;

        // Baza je upravo prihvatila ovo polje kroz migraciju
        public string? ProfilnaSlikaBase64 { get; set; }

        [NotMapped]
        public IFormFile? DatotekaSlike { get; set; }
    }
}