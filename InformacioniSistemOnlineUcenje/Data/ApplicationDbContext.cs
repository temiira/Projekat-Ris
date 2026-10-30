using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InformacioniSistemOnlineUcenje.Models;

namespace InformacioniSistemOnlineUcenje.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kurs> Kursevi { get; set; }
        public DbSet<Lekcija> Lekcije { get; set; }
        public DbSet<Upis> Upisi { get; set; }
        public DbSet<Napredak> Napredak { get; set; }
        public DbSet<Screenshot> Screenshots { get; set; }

        // Ako stari kod traži specifičan entitet Korisnik preko _context.Korisnici
        public DbSet<Korisnik> Korisnici { get; set; }

        // Ako stari kod traži LekcijaStatusi iz ranijih labova, mapiramo ga ovdje da izbjegnemo grešku
        public DbSet<LekcijaStatus> LekcijaStatusi { get; set; }
    }
}