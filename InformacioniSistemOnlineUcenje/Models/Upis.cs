using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InformacioniSistemOnlineUcenje.Models
{
    public class Upis
    {
        [Key]
        public int UpisID { get; set; }

        [Required]
        public int KorisnikID { get; set; }

        [ForeignKey("KorisnikID")]
        public Korisnik? Korisnik { get; set; }

        [Required]
        public int KursID { get; set; }

        [ForeignKey("KursID")]
        public Kurs? Kurs { get; set; }

        public DateTime DatumUpisa { get; set; } = DateTime.Now;
    }
}
