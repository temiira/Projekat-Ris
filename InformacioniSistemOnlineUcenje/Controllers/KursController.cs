using InformacioniSistemOnlineUcenje.Data;
using InformacioniSistemOnlineUcenje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InformacioniSistemOnlineUcenje.Controllers
{
    [Authorize] // Korisnik mora biti ulogovan
    public class KursController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public KursController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Pomoćna metoda za osiguravanje da korisnik uvijek postoji u tabeli Korisnici
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

        // Pregled svih kurseva
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var kurseviQuery = _context.Kursevi.Include(k => k.Lekcije).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                kurseviQuery = kurseviQuery.Where(s => s.Naziv.Contains(searchString) || s.Opis.Contains(searchString));
            }

            var kursevi = await kurseviQuery.ToListAsync();

            var trenutniKorisnik = await GetOrCreateTrenutniKorisnikAsync();
            int userIdInt = trenutniKorisnik != null ? trenutniKorisnik.KorisnikID : 1;

            var napredakDict = await _context.Napredak
                .Where(n => n.KorisnikID == userIdInt)
                .ToDictionaryAsync(n => n.KursID, n => n.Procenat);

            ViewBag.NapredakDict = napredakDict;
            ViewBag.PrikažiKreiraj = User.IsInRole("Admin") || User.IsInRole("Nastavnik") || (User.Identity?.Name == "ADMIN@E.BA");

            return View(kursevi);
        }

        // Detalji kursa
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var kurs = await _context.Kursevi
                .Include(k => k.Lekcije)
                .Include(k => k.Nastavnik)
                .FirstOrDefaultAsync(m => m.KursID == id);

            if (kurs == null) return NotFound();

            var trenutniKorisnik = await GetOrCreateTrenutniKorisnikAsync();
            int userIdInt = trenutniKorisnik != null ? trenutniKorisnik.KorisnikID : 1;

            bool upisan = await _context.Upisi
                .AnyAsync(u => u.KorisnikID == userIdInt && u.KursID == id);

            ViewBag.Upisan = upisan;

            bool jeNastavnikIliAdmin =
                (trenutniKorisnik != null && (trenutniKorisnik.Uloga == "Nastavnik" || trenutniKorisnik.Uloga == "Admin"))
                || User.IsInRole("Nastavnik")
                || User.IsInRole("Admin");

            ViewBag.JeNastavnikIliAdmin = jeNastavnikIliAdmin;

            return View(kurs);
        }

        // POST: Upis na kurs (Rješava 404 grešku ukoliko je dugme pozivalo upis)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpisiNaKurs(int kursId)
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

            return RedirectToAction(nameof(Details), new { id = kursId });
        }

        // GET: Kurs/Create
        [Authorize(Roles = "Admin,Nastavnik")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Nastavnici = new SelectList(_context.Korisnici.Where(k => k.Uloga == "Nastavnik"), "KorisnikID", "Ime");
            return View();
        }

        // POST: Kurs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Nastavnik")]
        public async Task<IActionResult> Create(Kurs kurs)
        {
            if (ModelState.IsValid)
            {
                if (kurs.NastavnikID == null || kurs.NastavnikID == 0)
                {
                    var trenutniKorisnik = await GetOrCreateTrenutniKorisnikAsync();
                    if (trenutniKorisnik != null)
                    {
                        kurs.NastavnikID = trenutniKorisnik.KorisnikID;
                    }
                }

                _context.Add(kurs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Nastavnici = new SelectList(_context.Korisnici.Where(k => k.Uloga == "Nastavnik"), "KorisnikID", "Ime");
            return View(kurs);
        }

        // Pregled prijavljenih učenika na kursu
        [Authorize(Roles = "Admin,Nastavnik")]
        [HttpGet]
        public async Task<IActionResult> PrikazUcenika(int id)
        {
            var kurs = await _context.Kursevi
                .FirstOrDefaultAsync(k => k.KursID == id);

            if (kurs == null) return NotFound();

            var upisaniUcenici = await _context.Upisi
                .Where(u => u.KursID == id)
                .Include(u => u.Korisnik)
                .ToListAsync();

            ViewBag.NazivKursa = kurs.Naziv;
            return View(upisaniUcenici);
        }
    }
}