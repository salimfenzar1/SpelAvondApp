using Microsoft.EntityFrameworkCore;
using SpelAvondApp.Domain.Models;
using SpelAvondApp.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SpellenRepository : ISpellenRepository
{
    private readonly SpellenDbContext _context;

    public SpellenRepository(SpellenDbContext context)
    {
        _context = context;
    }

    // Bordspel Methods
    public async Task AddBordspelAsync(Bordspel bordspel)
    {
        await _context.Bordspellen.AddAsync(bordspel);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBordspelAsync(Bordspel bordspel)
    {
        _context.Bordspellen.Update(bordspel);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBordspelAsync(int id)
    {
        var bordspel = await GetBordspelByIdAsync(id);
        if (bordspel != null)
        {
            _context.Bordspellen.Remove(bordspel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Bordspel> GetBordspelByIdAsync(int id)
    {
        return await _context.Bordspellen.FindAsync(id);
    }

    public async Task<List<Bordspel>> GetAllBordspellenAsync()
    {
        return await _context.Bordspellen.ToListAsync();
    }

    public async Task<List<Bordspel>> GetBordspellenByIdsAsync(List<int> ids)
    {
        return await _context.Bordspellen.Where(b => ids.Contains(b.Id)).ToListAsync();
    }

    // BordspellenAvond Methods
    public async Task AddBordspellenAvondAsync(BordspellenAvond avond)
    {
        await _context.BordspellenAvonden.AddAsync(avond);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBordspellenAvondAsync(BordspellenAvond avond)
    {
        var bestaandeAvond = await _context.BordspellenAvonden
            .Include(b => b.Bordspellen)
            .FirstOrDefaultAsync(a => a.Id == avond.Id);

        if (bestaandeAvond != null)
        {
            // Update de overige eigenschappen van de avond
            bestaandeAvond.Datum = avond.Datum;
            bestaandeAvond.Adres = avond.Adres;
            bestaandeAvond.MaxAantalSpelers = avond.MaxAantalSpelers;
            bestaandeAvond.Is18Plus = avond.Is18Plus;

            // Verwijder bestaande bordspellen en voeg de nieuwe toe
            bestaandeAvond.Bordspellen.Clear();
            var nieuweBordspellen = await GetBordspellenByIdsAsync(avond.Bordspellen.Select(b => b.Id).ToList());
            foreach (var bordspel in nieuweBordspellen)
            {
                bestaandeAvond.Bordspellen.Add(bordspel);
            }

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteBordspellenAvondAsync(int id)
    {
        var avond = await GetBordspellenAvondByIdAsync(id);
        if (avond != null)
        {
            _context.BordspellenAvonden.Remove(avond);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<BordspellenAvond> GetBordspellenAvondByIdAsync(int id)
    {
        return await _context.BordspellenAvonden
            .AsNoTracking()
            .Include(b => b.Organisator)
            .Include(b => b.Bordspellen) // Gebruik meervoud voor de lijst van bordspellen
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<BordspellenAvond>> GetAllBordspellenAvondenAsync()
    {
        return await _context.BordspellenAvonden
            .Include(b => b.Bordspellen) // Include the Bordspellen relation
            .Include(b => b.Organisator) // If you need the Organisator info as well
            .AsNoTracking()
            .ToListAsync();
    }


}
