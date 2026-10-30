using System;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemOnlineUcenje.Models
{
    public class LekcijaStatus
    {
        [Key]
        public int ID { get; set; }
        public string KorisnikID { get; set; }
        public int LekcijaID { get; set; }
        public bool IsProcitano { get; set; }
        public DateTime DatumZavrsetka { get; set; } = DateTime.Now;
    }
}