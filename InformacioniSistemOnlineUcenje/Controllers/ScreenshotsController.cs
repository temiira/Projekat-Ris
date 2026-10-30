using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using InformacioniSistemOnlineUcenje.Data;
using InformacioniSistemOnlineUcenje.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InformacioniSistemOnlineUcenje.Controllers
{
    public class ScreenshotController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScreenshotController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var screenshots = await _context.Screenshots.Include(s => s.Kurs).ToListAsync();
            return View(screenshots);
        }

        public IActionResult Create()
        {
            ViewBag.Kursevi = _context.Kursevi.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Screenshot model, IFormFile DatotekaSlike)
        {
            if (DatotekaSlike != null && DatotekaSlike.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await DatotekaSlike.CopyToAsync(memoryStream);
                    model.SlikaBajtovi = memoryStream.ToArray();
                }

                _context.Screenshots.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Kursevi = _context.Kursevi.ToList();
            ModelState.AddModelError("", "Molimo odaberite ispravnu sliku.");
            return View(model);
        }

        public async Task<IActionResult> PrikaziSliku(int id)
        {
            var screenshot = await _context.Screenshots.FirstOrDefaultAsync(s => s.ScreenshotID == id || s.ID == id);
            if (screenshot != null && screenshot.SlikaBajtovi != null)
            {
                return File(screenshot.SlikaBajtovi, "image/jpeg");
            }
            return NotFound();
        }
    }
}