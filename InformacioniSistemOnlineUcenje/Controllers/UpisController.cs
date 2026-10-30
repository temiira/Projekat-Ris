using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InformacioniSistemOnlineUcenje.Data;
using InformacioniSistemOnlineUcenje.Models;

namespace InformacioniSistemOnlineUcenje.Controllers
{
    [Authorize] // Samo prijavljeni korisnici mogu pristupiti
    public class UpisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UpisController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Pomoćna metoda za osiguravanje da korisnik postoji u tabeli Korisnici
        private async Task<Korisnik> GetOrCreateTrenutniKorisnikAsync()
        {
            var emailUlogovanog = User.Identity?.Name;
            if (string.IsNullOrEmpty(emailUlogovanog)) return null;

            var trenutniKorisnik = await _context.Korisnici.FirstOrDefaultAsync(k => k.Email == emailUlogovanog);
            if (trenutniKorisnik == null)
            {
                trenutniKorisnik = new Korisnik
                {
                    Ime = "Korisnik",
                    Prezime = "Sistema",
                    Email = emailUlogovanog,
                    Uloga = User.IsInRole("Admin") ? "Admin" : (User.IsInRole("Nastavnik") ? "Nastavnik" : "Ucenik")
                };
                _context.Korisnici.Add(trenutniKorisnik);
                await _context.SaveChangesAsync();
            }
            return trenutniKorisnik;
        }

        // 📌 OVO JE METODA KOJU JE FORMA TRAŽILA I PROUZROKOVALA 404!
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpisiKurs(int kursId)
        {
            var trenutniKorisnik = await GetOrCreateTrenutniKorisnikAsync();
            if (trenutniKorisnik == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            bool vecUpisan = await _context.Upisi.AnyAsync(u => u.KorisnikID == trenutniKorisnik.KorisnikID && u.KursID == kursId);
            if (!vecUpisan)
            {
                var noviUpis = new Upis
                {
                    KorisnikID = trenutniKorisnik.KorisnikID,
                    KursID = kursId,
                    DatumUpisa = DateTime.Now
                };
                _context.Upisi.Add(noviUpis);
                await _context.SaveChangesAsync();
            }

            // Vrati korisnika nazad na detalje kursa ili na njegove kurseve
            return RedirectToAction("Details", "Kurs", new { id = kursId });
        }

        public async Task<IActionResult> MojiKursevi()
        {
            var trenutniKorisnik = await GetOrCreateTrenutniKorisnikAsync();
            if (trenutniKorisnik == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            int userIdInt = trenutniKorisnik.KorisnikID;
            string userIdString = trenutniKorisnik.KorisnikID.ToString();

            var upisi = await _context.Upisi
                .Include(u => u.Kurs)
                    .ThenInclude(k => k.Lekcije)
                .Include(u => u.Kurs)
                    .ThenInclude(k => k.Nastavnik)
                .Where(u => u.KorisnikID == userIdInt)
                .ToListAsync();

            var ukupnoLekcijaDict = new Dictionary<int, int>();
            var procitanoLekcijaDict = new Dictionary<int, int>();

            foreach (var upis in upisi)
            {
                int kursId = upis.KursID;
                int ukupnoLekcija = upis.Kurs?.Lekcije?.Count ?? 0;

                var lekcijeIDs = upis.Kurs?.Lekcije?.Select(l => l.LekcijaID).ToList() ?? new List<int>();

                int procitanoLekcija = await _context.LekcijaStatusi
                    .CountAsync(s => (s.KorisnikID.ToString() == userIdString) && s.IsProcitano && lekcijeIDs.Contains(s.LekcijaID));

                ukupnoLekcijaDict[kursId] = ukupnoLekcija;
                procitanoLekcijaDict[kursId] = procitanoLekcija;
            }

            ViewBag.UkupnoLekcija = ukupnoLekcijaDict;
            ViewBag.ProcitanoLekcija = procitanoLekcijaDict;

            return View(upisi);
        }
    }
}