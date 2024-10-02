using Microsoft.EntityFrameworkCore;
//using SpelAvondApp.Domain.Models; // Zorg dat je modelbestanden importeert

namespace SpelAvondApp.Infrastructure
{
    public class SpellenDbContext : DbContext
    {
        public SpellenDbContext(DbContextOptions<SpellenDbContext> options)
            : base(options)
        {
        }

        //public DbSet<Bordspel> Bordspellen { get; set; }
        //public DbSet<BordspellenAvond> BordspellenAvonden { get; set; }
    }
}