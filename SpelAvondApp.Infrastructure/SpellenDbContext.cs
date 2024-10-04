using Microsoft.EntityFrameworkCore;
using SpelAvondApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace SpelAvondApp.Infrastructure
{
    public class SpellenDbContext : DbContext
    {
        public SpellenDbContext(DbContextOptions<SpellenDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bordspel> Bordspellen { get; set; }
        public DbSet<BordspellenAvond> BordspellenAvonden { get; set; }
        public DbSet<Inschrijving> Inschrijvingen { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Veel-op-veel relatie tussen BordspellenAvond en Bordspel
            modelBuilder.Entity<BordspellenAvond>()
                .HasMany(b => b.Bordspellen)
                .WithMany(s => s.BordspellenAvonden)
                .UsingEntity(j => j.ToTable("BordspellenAvondBordspellen"));

            // Eén-op-veel relatie tussen BordspellenAvond en Inschrijving
            modelBuilder.Entity<BordspellenAvond>()
                .HasMany(b => b.Inschrijvingen)
                .WithOne(i => i.BordspellenAvond)
                .HasForeignKey(i => i.BordspellenAvondId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete: als een BordspellenAvond wordt verwijderd, verwijder alle inschrijvingen

            // Eén-op-veel relatie tussen BordspellenAvond en Review
            modelBuilder.Entity<BordspellenAvond>()
                .HasMany(b => b.Reviews)
                .WithOne(r => r.BordspellenAvond)
                .HasForeignKey(r => r.BordspellenAvondId)
                .OnDelete(DeleteBehavior.Cascade);

            // Eén-op-veel relatie tussen Speler (IdentityUser) en Inschrijving
            modelBuilder.Entity<Inschrijving>()
                .HasOne(i => i.Speler)
                .WithMany() // Eén speler kan meerdere inschrijvingen hebben
                .HasForeignKey(i => i.SpelerId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict: als een speler wordt verwijderd, verwijder niet automatisch de inschrijving

            // Eén-op-veel relatie tussen Speler (IdentityUser) en Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Speler)
                .WithMany() // Eén speler kan meerdere reviews hebben
                .HasForeignKey(r => r.SpelerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
