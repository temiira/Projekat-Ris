using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InformacioniSistemOnlineUcenje.Models
{
    public class Screenshot
    {
        [Key]
        public int ScreenshotID { get; set; }

        [NotMapped]
        public int ID
        {
            get => ScreenshotID;
            set => ScreenshotID = value;
        }

        [Required]
        public byte[] SlikaBajtovi { get; set; }

        [NotMapped]
        public byte[] Slika
        {
            get => SlikaBajtovi;
            set => SlikaBajtovi = value;
        }

        // DODANO: Polje Opis koje sistem traži u greškama
        public string Opis { get; set; } = string.Empty;

        [ForeignKey("Kurs")]
        public int KursID { get; set; }
        public virtual Kurs Kurs { get; set; }

        [NotMapped]
        public Microsoft.AspNetCore.Http.IFormFile DatotekaSlike { get; set; }
    }
}