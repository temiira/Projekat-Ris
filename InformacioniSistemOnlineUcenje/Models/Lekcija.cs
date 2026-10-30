using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InformacioniSistemOnlineUcenje.Models
{
    public class Lekcija
    {
        [Key]
        public int LekcijaID { get; set; }

        public int KursID { get; set; }

        // Ako se u bazi kolona zove Naziv, a u kodu želiš ispis, mapiramo je ovako:
        [Column("Naziv")]
        public string? Naziv { get; set; }

        public string? Sadrzaj { get; set; }

        public string? SlikaBase64 { get; set; }

        public DateTime? DatumDodavanja { get; set; }

        // POSEBAN TRIK: Ako baza nema kolone 'Naslov' i 'RedniBroj', 
        // stavljamo [NotMapped] kako ih Entity Framework uopšte ne bi tražio u SQL upitu!
        [NotMapped]
        public string? Naslov { get; set; }

        [NotMapped]
        public int? RedniBroj { get; set; }

        [ForeignKey("KursID")]
        public virtual Kurs? Kurs { get; set; }
    }
}