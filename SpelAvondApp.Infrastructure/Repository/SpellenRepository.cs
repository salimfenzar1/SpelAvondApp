using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpelAvondApp.Domain.Models;
using SpelAvondApp.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SpellenRepository : ISpellenRepository
{
    private readonly SpellenDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;


    public SpellenRepository(SpellenDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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
            // Update de eigenschappen van de avond
            bestaandeAvond.Datum = avond.Datum;
            bestaandeAvond.Adres = avond.Adres;
            bestaandeAvond.MaxAantalSpelers = avond.MaxAantalSpelers;
            bestaandeAvond.Is18Plus = avond.Is18Plus;
            bestaandeAvond.OrganisatorId = avond.OrganisatorId;

            // Update bordspellen voor de avond
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
            .Include(b => b.Bordspellen) // Include de lijst van bordspellen
            .Include(a => a.Inschrijvingen)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<BordspellenAvond>> GetAllBordspellenAvondenAsync()
    {
        return await _context.BordspellenAvonden
            .Include(b => b.Bordspellen) // Include de Bordspellen relatie
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddInschrijvingAsync(Inschrijving inschrijving)
    {
        _context.Inschrijvingen.Add(inschrijving);
        await _context.SaveChangesAsync();
    }
    public async Task<Inschrijving> GetInschrijvingAsync(string userId, int avondId)
    {
        return await _context.Inschrijvingen
            .FirstOrDefaultAsync(i => i.SpelerId == userId && i.BordspellenAvondId == avondId);
    }

    public async Task<BordspellenAvond> GetAvondWithInschrijvingenAsync(int id)
    {
        var avond = await _context.BordspellenAvonden
            .Include(a => a.Inschrijvingen)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (avond != null)
        {
            foreach (var inschrijving in avond.Inschrijvingen)
            {
                inschrijving.Speler = await _userManager.FindByIdAsync(inschrijving.SpelerId);
            }
        }

        return avond;
    }

    public async Task<List<BordspellenAvond>> GetAvondenByOrganisatorAsync(string organisatorId)
    {
        return await _context.BordspellenAvonden
            .Where(a => a.OrganisatorId == organisatorId)
            .Include(a => a.Inschrijvingen)
            .ToListAsync();
    }

}
