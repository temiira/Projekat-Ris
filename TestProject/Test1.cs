using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InformacioniSistemOnlineUcenje.Controllers;
using InformacioniSistemOnlineUcenje.Data;
using InformacioniSistemOnlineUcenje.Models;
using System;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class Test1
    {
        // Pomoćna metoda koja kreira potpuno novu, čistu bazu u memoriji za svaki test posebno
        private ApplicationDbContext GenerisiCistiContext(string imeBaze)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: imeBaze)
                .Options;

            var context = new ApplicationDbContext(options);

            // Odmah ubacujemo testnog korisnika u tu novu privremenu bazu
            context.Korisnici.Add(new Korisnik
            {
                KorisnikID = 1,
                Ime = "Ema",
                Prezime = "Test",
                Email = "ema@test.ba",
                Lozinka = "Sifra123!",
                Uloga = "Učenik",
                DatumRegistracije = DateTime.Now
            });
            context.SaveChanges();

            return context;
        }

        // TEST 1: Provjera Details rute kada korisnik POSTOJI u bazi -> Mora vratiti ViewResult
        [TestMethod]
        public async Task Details_KadaKorisnikPostoji_VracaViewResult()
        {
            // Arrange
            using (var context = GenerisiCistiContext("Baza_Test_Postoji"))
            {
                var controller = new KorisnikController(context, null, null);

                // Act
                var result = await controller.Details(1);

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }
        }

        // TEST 2: Provjera Details rute kada korisnik NE POSTOJI u bazi -> Mora vratiti NotFoundResult
        [TestMethod]
        public async Task Details_KadaKorisnikNePostoji_VracaNotFound()
        {
            // Arrange
            using (var context = GenerisiCistiContext("Baza_Test_NePostoji"))
            {
                var controller = new KorisnikController(context, null, null);

                // Act
                var result = await controller.Details(999); // ID koji sigurno ne postoji

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }
        }

        // TEST 3: Provjera ponašanja kada model prilikom kreiranja novog korisnika nije validan
        [TestMethod]
        public async Task Create_KadaModelNijeValidan_VracaIstiView()
        {
            // Arrange
            using (var context = GenerisiCistiContext("Baza_Test_Model"))
            {
                var controller = new KorisnikController(context, null, null);

                // Simuliramo nevalidno stanje (npr. nedostaju obavezna polja)
                controller.ModelState.AddModelError("Ime", "Ime je obavezno");
                var nevalidanKorisnik = new Korisnik { Prezime = "Test Bez Imena" };

                // Act
                var result = await controller.Create(nevalidanKorisnik);

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }
        }
        // TEST 4: Provjera uspješnog kreiranja novog korisnika (POST Create)
        [TestMethod]
        public async Task Create_KadaJeModelValidan_UspjesnoDodajeIPreusmjerava()
        {
            // Arrange
            using (var context = GenerisiCistiContext("Baza_Test_Create_Validan"))
            {
                var controller = new KorisnikController(context, null, null);
                var noviKorisnik = new Korisnik
                {
                    KorisnikID = 2,
                    Ime = "Dino",
                    Prezime = "Dinić",
                    Email = "dino@test.ba",
                    Lozinka = "Sifra123!",
                    Uloga = "Učenik",
                    DatumRegistracije = DateTime.Now
                };

                // Act
                var result = await controller.Create(noviKorisnik);

                // Assert
                // Očekujemo da nakon uspješnog unosa preusmjerava na Index akciju (RedirectToActionResult)
                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            }
        }
    }
}