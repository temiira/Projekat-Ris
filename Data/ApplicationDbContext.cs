using InformacioniSistemOnlineUcenje.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InformacioniSistemOnlineUcenje.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Kurs> Kursevi { get; set; }
        public DbSet<Lekcija> Lekcije { get; set; }
        public DbSet<Upis> Upisi { get; set; }
        public DbSet<Napredak> Napredak { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Kurs ↔ Korisnik (Nastavnik)
            modelBuilder.Entity<Kurs>()
                .HasOne(k => k.Nastavnik)
                .WithMany()
                .HasForeignKey(k => k.NastavnikID)
                .OnDelete(DeleteBehavior.NoAction);

            // 🔹 Lekcija ↔ Kurs
            modelBuilder.Entity<Lekcija>()
                .HasOne(l => l.Kurs)
                .WithMany()
                .HasForeignKey(l => l.KursID)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Upis ↔ Korisnik
            modelBuilder.Entity<Upis>()
                .HasOne(u => u.Korisnik)
                .WithMany()
                .HasForeignKey(u => u.KorisnikID)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Upis ↔ Kurs
            modelBuilder.Entity<Upis>()
                .HasOne(u => u.Kurs)
                .WithMany()
                .HasForeignKey(u => u.KursID)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Napredak ↔ Korisnik
            modelBuilder.Entity<Napredak>()
                .HasOne(n => n.Korisnik)
                .WithMany()
                .HasForeignKey(n => n.KorisnikID)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Napredak ↔ Kurs
            modelBuilder.Entity<Napredak>()
                .HasOne(n => n.Kurs)
                .WithMany()
                .HasForeignKey(n => n.KursID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
