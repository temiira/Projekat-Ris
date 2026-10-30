using System;
using System.Collections.Generic;

namespace InformacioniSistemOnlineUcenje.Models
{
    public class NapredakKursaVM
    {
        public int KursID { get; set; }
        public string NazivKursa { get; set; }
        public string Nastavnik { get; set; }

        public int UkupnoLekcija { get; set; }
        public int ZavrsenoLekcija { get; set; }
        public double Procenat { get; set; }
    }

    public class NapredakDashboardVM
    {
        public int UkupnoKurseva { get; set; }
        public int UkupnoLekcija { get; set; }
        public int UkupnoZavrseno { get; set; }

        public double ProcenatUkupno
        {
            get
            {
                if (UkupnoLekcija == 0)
                    return 0;

                return (double)UkupnoZavrseno / UkupnoLekcija * 100.0;
            }
        }

        public string? ZadnjaLekcijaNaziv { get; set; }
        public string? ZadnjiKursNaziv { get; set; }
        public DateTime? ZadnjeVrijeme { get; set; }

        // ✔ OVO JE DODANO — ime isto kao što ti koristiš
        public List<NapredakKursaVM> Kursevi { get; set; } = new();
    }
}
