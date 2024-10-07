using Microsoft.EntityFrameworkCore;
using SpelAvondApp.Domain.Models;
using SpelAvondApp.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SpellenRepository : ISpellenRepository
{
    private readonly SpellenDbContext _context;

    public SpellenRepository(SpellenDbContext context)
    {
        _context = context;
    }

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
}
