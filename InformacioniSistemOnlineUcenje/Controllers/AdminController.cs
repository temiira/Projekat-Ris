using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using InformacioniSistemOnlineUcenje.Data;
using InformacioniSistemOnlineUcenje.Models;

namespace InformacioniSistemOnlineUcenje.Controllers
{
    [Authorize(Roles = "Admin")] // Samo Administrator može pristupiti
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Prikaz svih korisnika u sistemu
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // Brisanje korisnika
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["Poruka"] = "Korisnik je uspješno obrisan!";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // 📌 Generisanje / pregled izvještaja sistema
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Izvjestaji()
        {
            ViewBag.UkupnoKurseva = await _context.Kursevi.CountAsync();
            ViewBag.UkupnoKorisnika = await _context.Korisnici.CountAsync();
            ViewBag.UkupnoUpisa = await _context.Upisi.CountAsync();

            // Svi upisi za detaljan izvještaj
            var sviUpisi = await _context.Upisi
                .Include(u => u.Kurs)
                .Include(u => u.Korisnik)
                .ToListAsync();

            return View(sviUpisi);
        }
    }
}