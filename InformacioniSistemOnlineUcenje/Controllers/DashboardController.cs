using InformacioniSistemOnlineUcenje.Data;
using Microsoft.AspNetCore.Mvc;

namespace InformacioniSistemOnlineUcenje.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var korisnik = _context.Korisnici.FirstOrDefault(x => x.KorisnikID == userId);

            return View(korisnik);
        }
    }
}
