using InformacioniSistemOnlineUcenje.Data;
using InformacioniSistemOnlineUcenje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace InformacioniSistemOnlineUcenje.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Slajd 58 & 59: Automatsko dodavanje registrovanog korisnika u našu tabelu Korisnik
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var email = User.Identity.Name;

                // Provjeravamo da li veæ postoji u tabeli Korisnici
                var korisnikPostoji = _context.Korisnici.Any(k => k.Email == email);

                if (!korisnikPostoji)
                {
                    var noviKorisnik = new Korisnik
                    {
                        Ime = email.Split('@')[0], // Uzima dio prije @ kao ime privremeno
                        Prezime = "Korisnik",
                        Email = email,
                        Lozinka = "Predefinisana123!",
                        Uloga = "Uèenik", // Predefinisana uloga prema tvojoj formi
                        DatumRegistracije = DateTime.Now
                    };

                    _context.Korisnici.Add(noviKorisnik);
                    _context.SaveChanges();
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}