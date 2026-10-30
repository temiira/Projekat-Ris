using InformacioniSistemOnlineUcenje.Data;
using InformacioniSistemOnlineUcenje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InformacioniSistemOnlineUcenje.Controllers
{
    public class LekcijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LekcijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 Vidljivo svima
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var list = await _context.Lekcije
                .Include(l => l.Kurs)
                .OrderByDescending(l => l.DatumDodavanja)
                .ToListAsync();

            return View(list);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var lekcija = await _context.Lekcije
                .Include(l => l.Kurs)
                .FirstOrDefaultAsync(l => l.LekcijaID == id);

            if (lekcija == null)
                return NotFound();

            var userId = HttpContext.Session.GetInt32("UserId");

            bool moze = false;
            bool procitano = false;
            if (userId != null)
            {
                string userIdString = userId.ToString();

                moze = await _context.Upisi
                    .AnyAsync(u => u.KorisnikID.ToString() == userIdString && u.KursID == lekcija.KursID);

                procitano = await _context.LekcijaStatusi
                    .AnyAsync(s => s.KorisnikID == userIdString && s.LekcijaID == id && s.IsProcitano);
            }

            ViewBag.MozeOznaciti = moze && !procitano;
            ViewBag.Procitano = procitano;

            return View(lekcija);
        }

        // 📌 OZNAČAVANJE LEKCIJE KAO PROČITANE (Ispravljeno na IsProcitano)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Procitano(int id)
        {
            var lekcija = await _context.Lekcije.FindAsync(id);
            if (lekcija == null)
                return NotFound();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Identity/Account/Login");

            string userIdString = userId.ToString();

            // Provjeri da li već postoji zapis
            var postojeciZapis = await _context.LekcijaStatusi
                .FirstOrDefaultAsync(s => s.LekcijaID == id && s.KorisnikID == userIdString);

            if (postojeciZapis == null)
            {
                var noviStatus = new LekcijaStatus
                {
                    LekcijaID = id,
                    KorisnikID = userIdString,
                    IsProcitano = true, // Ispravljeno na tačan naziv iz modela
                    DatumZavrsetka = DateTime.Now
                };

                _context.LekcijaStatusi.Add(noviStatus);
                await _context.SaveChangesAsync(); // Ispravno sa await
            }
            else if (!postojeciZapis.IsProcitano)
            {
                postojeciZapis.IsProcitano = true;
                postojeciZapis.DatumZavrsetka = DateTime.Now;
                _context.Update(postojeciZapis);
                await _context.SaveChangesAsync(); // Ispravno sa await
            }

            return RedirectToAction("Details", new { id = id });
        }

        // 📌 DODAVANJE LEKCIJE
        [Authorize(Roles = "Nastavnik,Admin")]
        public IActionResult Create()
        {
            ViewBag.Kursevi = new SelectList(_context.Kursevi, "KursID", "Naziv");
            return View();
        }

        [Authorize(Roles = "Nastavnik,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lekcija model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kursevi = new SelectList(_context.Kursevi, "KursID", "Naziv");
                return View(model);
            }

            model.DatumDodavanja = DateTime.Now;

            _context.Lekcije.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Kurs", new { id = model.KursID });
        }

        // 📌 UREĐIVANJE LEKCIJE (GET)
        [Authorize(Roles = "Nastavnik,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var lekcija = await _context.Lekcije.FindAsync(id);
            if (lekcija == null) return NotFound();

            ViewBag.Kursevi = new SelectList(_context.Kursevi, "KursID", "Naziv", lekcija.KursID);
            return View(lekcija);
        }

        // 📌 UREĐIVANJE LEKCIJE (POST)
        [Authorize(Roles = "Nastavnik,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lekcija model)
        {
            if (id != model.LekcijaID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Lekcije.Any(e => e.LekcijaID == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction("Details", "Kurs", new { id = model.KursID });
            }

            ViewBag.Kursevi = new SelectList(_context.Kursevi, "KursID", "Naziv", model.KursID);
            return View(model);
        }

        // 📌 BRISANJE LEKCIJE (GET)
        [Authorize(Roles = "Nastavnik,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var lekcija = await _context.Lekcije
                .Include(l => l.Kurs)
                .FirstOrDefaultAsync(m => m.LekcijaID == id);

            if (lekcija == null) return NotFound();

            return View(lekcija);
        }

        // 📌 BRISANJE LEKCIJE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Nastavnik,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lekcija = await _context.Lekcije.FindAsync(id);
            int kursId = lekcija != null ? lekcija.KursID : 0;

            if (lekcija != null)
            {
                _context.Lekcije.Remove(lekcija);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Kurs", new { id = kursId });
        }
    }
}