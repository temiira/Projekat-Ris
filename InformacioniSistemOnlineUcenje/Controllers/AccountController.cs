using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using InformacioniSistemOnlineUcenje.Data;
using InformacioniSistemOnlineUcenje.Models;
using System.Linq;
using System;

namespace InformacioniSistemOnlineUcenje.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // =====================================
        // LOGIN (PRIJAVA)
        // =====================================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string lozinka)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(lozinka))
            {
                ModelState.AddModelError(string.Empty, "Email i lozinka su obavezni.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Korisnik sa tim emailom ne postoji.");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, lozinka, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // 🔥 SINHRONIZACIJA SA SESIJOM: Pronalazimo korisnika u vašoj Korisnici tabeli i stavljamo ID u sesiju
                var mojKorisnik = _context.Korisnici.FirstOrDefault(k => k.Email == email);
                if (mojKorisnik != null)
                {
                    HttpContext.Session.SetInt32("UserId", mojKorisnik.KorisnikID);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Neispravna lozinka.");
            return View();
        }

        // =====================================
        // REGISTER (REGISTRACIJA)
        // =====================================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string ime, string prezime, string email, string lozinka, string uloga)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(lozinka) || string.IsNullOrEmpty(ime) || string.IsNullOrEmpty(prezime))
            {
                ModelState.AddModelError(string.Empty, "Sva polja su obavezna.");
                return View();
            }

            // 1. Kreiramo korisnika u ugrađenom Identity sistemu
            var identityUser = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(identityUser, lozinka);

            if (result.Succeeded)
            {
                // 🎯 Automatsko dodjeljivanje uloge u Identity sistemu
                string dodijeljenaUloga = string.IsNullOrEmpty(uloga) ? "Ucenik" : uloga;
                await _userManager.AddToRoleAsync(identityUser, dodijeljenaUloga);

                // 2. Kreiramo korisnika u vašoj prilagođenoj tabeli 'Korisnici'
                var noviKorisnik = new Korisnik
                {
                    Ime = ime,
                    Prezime = prezime,
                    Email = email,
                    Lozinka = lozinka, // Prema Vašem modelu
                    Uloga = dodijeljenaUloga,
                    DatumRegistracije = DateTime.Now
                };

                _context.Korisnici.Add(noviKorisnik);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetInt32("UserId", noviKorisnik.KorisnikID);

                await _signInManager.SignInAsync(identityUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        // =====================================
        // LOGOUT (ODJAVA)
        // =====================================
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear(); // Brišemo sesiju pri odjavi
            return RedirectToAction("Index", "Home");
        }
    }
}