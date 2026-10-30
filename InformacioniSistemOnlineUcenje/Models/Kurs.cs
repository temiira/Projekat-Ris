using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InformacioniSistemOnlineUcenje.Models
{
    public class Kurs
    {
        [Key]
        public int KursID { get; set; }

        public string? Naziv { get; set; } // Promijenjeno u nullable

        public string? Opis { get; set; } // Promijenjeno u nullable

        [ForeignKey("Nastavnik")]
        public int? NastavnikID { get; set; } // Može biti null ako neki kurs nema nastavnika
        public virtual Korisnik? Nastavnik { get; set; }

        public virtual ICollection<Lekcija>? Lekcije { get; set; } = new List<Lekcija>();
    }
}