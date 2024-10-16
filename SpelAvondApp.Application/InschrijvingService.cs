using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpelAvondApp.Application;
using SpelAvondApp.Domain.Models;
using System.Threading.Tasks;


public class InschrijvingService : IInschrijvingService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISpellenRepository _repository;

    public InschrijvingService(UserManager<ApplicationUser> userManager, ISpellenRepository repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<bool> InschrijvenVoorAvondAsync(string userId, int avondId, string dieetWensen)
    {
        var bestaandeInschrijving = await _repository.GetInschrijvingAsync(userId, avondId);
        if (bestaandeInschrijving != null)
        {
            return false; 
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var avond = await _repository.GetBordspellenAvondByIdAsync(avondId);
        if (avond == null || avond.Inschrijvingen.Count >= avond.MaxAantalSpelers) return false;

        var inschrijving = new Inschrijving
        {
            SpelerId = userId,
            DieetWensen = dieetWensen,
            BordspellenAvondId = avondId,
            Aanwezig = true
        };

        await _repository.AddInschrijvingAsync(inschrijving);
        return true;
    }

    public async Task<bool> HeeftAlIngeschreven(string userId, int avondId)
    {
        var inschrijving = await _repository.GetInschrijvingAsync(userId, avondId);
        return inschrijving != null;
    }
    public async Task<BordspellenAvond> GetAvondWithInschrijvingenAndUserNamesAsync(int avondId)
    {
        var avond = await _repository.GetAvondWithInschrijvingenAsync(avondId);

        if (avond != null)
        {
            foreach (var inschrijving in avond.Inschrijvingen)
            {
                var user = await _userManager.FindByIdAsync(inschrijving.SpelerId);
                if (user != null)
                {
                    inschrijving.Speler = user; 
                }
                else
                {
                    inschrijving.Speler = new ApplicationUser { UserName = "Onbekend" }; 
                }
            }
        }

        return avond;
    }
    public async Task<BordspellenAvond> GetAvondMetDieetOptiesAsync(int avondId)
    {
        return await _repository.GetAvondMetDieetOptiesAsync(avondId);
    }
    public async Task<bool> KanDeelnemenAanAvond(ApplicationUser gebruiker, DateTime avondDatum)
    {
        return await _repository.HeeftInschrijvingOpDatumAsync(gebruiker.Id, avondDatum);
    }
}
