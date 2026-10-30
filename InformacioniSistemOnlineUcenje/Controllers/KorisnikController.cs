using InformacioniSistemOnlineUcenje.Data;
using InformacioniSistemOnlineUcenje.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace InformacioniSistemOnlineUcenje.Controllers
{
    public class KorisnikController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public KorisnikController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("Korisnik")]
        [Route("Korisnik/Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Korisnici.ToListAsync());
        }

        [HttpGet]
        [Route("Korisnik/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var korisnik = await _context.Korisnici.FirstOrDefaultAsync(k => k.KorisnikID == id);
            if (korisnik == null) return NotFound();
            return View(korisnik);
        }

        [HttpGet]
        [Route("Korisnik/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Korisnik/Create")]
        public async Task<IActionResult> Create(Korisnik korisnik)
        {
            if (!ModelState.IsValid) return View(korisnik);

            // 📸 LOGIKA ZA PROCESIRANJE SLIKE (Slajd 26)
            if (korisnik.DatotekaSlike != null && korisnik.DatotekaSlike.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await korisnik.DatotekaSlike.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();

                    // Pretvaranje u Base64String i spremanje u atribut modela
                    korisnik.ProfilnaSlikaBase64 = System.Convert.ToBase64String(fileBytes);
                }
            }

            _context.Korisnici.Add(korisnik);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Korisnik/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);
            if (korisnik == null) return NotFound();
            return View(korisnik);
        }

        [HttpPost]
        [Route("Korisnik/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Korisnik korisnik)
        {
            if (!ModelState.IsValid) return View(korisnik);

            // Obrada slike i prilikom uredi opcije
            if (korisnik.DatotekaSlike != null && korisnik.DatotekaSlike.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await korisnik.DatotekaSlike.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    korisnik.ProfilnaSlikaBase64 = System.Convert.ToBase64String(fileBytes);
                }
            }

            _context.Update(korisnik);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Korisnik/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);
            if (korisnik == null) return NotFound();
            return View(korisnik);
        }

        [HttpPost, ActionName("Delete")]
        [Route("Korisnik/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);
            if (korisnik != null)
            {
                _context.Korisnici.Remove(korisnik);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}