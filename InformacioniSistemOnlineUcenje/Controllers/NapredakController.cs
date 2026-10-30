using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InformacioniSistemOnlineUcenje.Data;
using InformacioniSistemOnlineUcenje.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InformacioniSistemOnlineUcenje.Controllers
{
    [Authorize]
    public class NapredakController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NapredakController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var emailUlogovanog = User.Identity?.Name;
            var trenutniKorisnik = await _context.Korisnici.FirstOrDefaultAsync(k => k.Email == emailUlogovanog);
            int userIdInt = trenutniKorisnik != null ? trenutniKorisnik.KorisnikID : 1;

            var progrese = await _context.Napredak
                .Where(n => n.KorisnikID == userIdInt)
                .Include(n => n.Kurs)
                .ToListAsync();

            return View(progrese);
        }

        [HttpPost]
        public async Task<IActionResult> OznaciProcitano(int lekcijaId, int kursId)
        {
            // 🎯 Dohvatanje trenutno ulogovanog korisnika
            var emailUlogovanog = User.Identity?.Name;
            var trenutniKorisnik = await _context.Korisnici.FirstOrDefaultAsync(k => k.Email == emailUlogovanog);
            int userIdInt = trenutniKorisnik != null ? trenutniKorisnik.KorisnikID : 1;
            var stringUserId = userIdInt.ToString();

            // 1. Provjera i upis u tabelu LekcijaStatusi
            var vecProcitano = await _context.LekcijaStatusi
                .AnyAsync(ls => ls.KorisnikID == stringUserId && ls.LekcijaID == lekcijaId);

            if (!vecProcitano)
            {
                var noviStatus = new LekcijaStatus
                {
                    KorisnikID = stringUserId,
                    LekcijaID = lekcijaId,
                    IsProcitano = true,
                    DatumZavrsetka = DateTime.Now
                };
                _context.LekcijaStatusi.Add(noviStatus);
                await _context.SaveChangesAsync();
            }

            // 2. Računanje ukupnog progresa na kursu
            var ukupnoLekcija = await _context.Lekcije.CountAsync(l => l.KursID == kursId);
            if (ukupnoLekcija > 0)
            {
                var sveLekcijeKursaIds = await _context.Lekcije
                    .Where(l => l.KursID == kursId)
                    .Select(l => l.LekcijaID)
                    .ToListAsync();

                var brojProvjerenih = await _context.LekcijaStatusi
                    .CountAsync(ls => ls.KorisnikID == stringUserId && sveLekcijeKursaIds.Contains(ls.LekcijaID) && ls.IsProcitano);

                int stvarniProcenat = (int)Math.Round((double)brojProvjerenih / ukupnoLekcija * 100);

                // 🎯 POPRAVLJENO: Zamijenjen kineski karakter sa ispravnim 'kursId'
                var napredak = await _context.Napredak
                    .FirstOrDefaultAsync(n => n.KorisnikID == userIdInt && n.KursID == kursId);

                if (napredak == null)
                {
                    napredak = new Napredak
                    {
                        KorisnikID = userIdInt,
                        KursID = kursId,
                        Procenat = stvarniProcenat,
                        DatumAzuriranja = DateTime.Now
                    };
                    _context.Napredak.Add(napredak);
                }
                else
                {
                    napredak.Procenat = stvarniProcenat;
                    napredak.DatumAzuriranja = DateTime.Now;
                    _context.Napredak.Update(napredak);
                }

                await _context.SaveChangesAsync();
            }

            // 3. Vraćanje nazad na detalje kursa
            return Redirect($"/Kurs/Details/{kursId}");
        }
    }
}