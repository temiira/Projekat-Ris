using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemOnlineUcenje.Models
{
    public class Napredak
    {
        public int NapredakID { get; set; }

        [Required]
        public int KorisnikID { get; set; }
        public Korisnik? Korisnik { get; set; }

        [Required]
        public int KursID { get; set; }
        public Kurs? Kurs { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Procenat mora biti između 0 i 100")]
        public int Procenat { get; set; }


        public DateTime DatumAzuriranja { get; set; } = DateTime.Now;
    }
}
