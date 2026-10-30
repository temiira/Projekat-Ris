using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InformacioniSistemOnlineUcenje.Data;
using InformacioniSistemOnlineUcenje.Models;

namespace InformacioniSistemOnlineUcenje.Controllers
{
    [Route("[controller]/[action]")]
    public class ProfilController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfilController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. Dobavljamo email trenutno ulogovanog korisnika (učenika)
            string? emailUlogovanog = User.Identity?.Name;

            // Ako sistem ne koristi standardni Identity, provjeri da li email čuvate u sesiji:
            if (string.IsNullOrEmpty(emailUlogovanog))
            {
                emailUlogovanog = HttpContext.Session.GetString("EmailKorisnika"); // Prilagodi ako je ključ drugačiji u labu
            }

            if (string.IsNullOrEmpty(emailUlogovanog))
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Tražimo BAŠ TOG korisnika u bazi, a ne prvog bilo kojeg!
            var korisnik = await _context.Korisnici.FirstOrDefaultAsync(k => k.Email == emailUlogovanog);

            if (korisnik == null) return NotFound();

            return View(korisnik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadSlike(Korisnik model)
        {
            string? emailUlogovanog = User.Identity?.Name ?? HttpContext.Session.GetString("EmailKorisnika");

            if (string.IsNullOrEmpty(emailUlogovanog)) return RedirectToAction("Login", "Account");

            // Osiguravamo da sliku snimamo ulogovanom učeniku
            var korisnik = await _context.Korisnici.FirstOrDefaultAsync(k => k.Email == emailUlogovanog);
            if (korisnik == null) return NotFound();

            if (model.DatotekaSlike != null && model.DatotekaSlike.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await model.DatotekaSlike.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    string base64String = Convert.ToBase64String(fileBytes);

                    korisnik.ProfilnaSlikaBase64 = base64String;
                }

                _context.Update(korisnik);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}