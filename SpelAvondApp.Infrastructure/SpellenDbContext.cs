using Microsoft.EntityFrameworkCore;
using SpelAvondApp.Domain.Models;

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

        modelBuilder.Entity<BordspellenAvond>()
            .HasMany(b => b.Bordspellen)
            .WithMany(s => s.BordspellenAvonden)
            .UsingEntity(j => j.ToTable("BordspellenAvondBordspellen"));

        modelBuilder.Entity<BordspellenAvond>()
            .HasMany(b => b.Inschrijvingen)
            .WithOne(i => i.BordspellenAvond)
            .HasForeignKey(i => i.BordspellenAvondId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BordspellenAvond>()
            .HasMany(b => b.Reviews)
            .WithOne(r => r.BordspellenAvond)
            .HasForeignKey(r => r.BordspellenAvondId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
