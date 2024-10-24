using Microsoft.AspNetCore.Identity;
using SpelAvondApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BordspellenAvondService : IBordspellenAvondService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISpellenRepository _repository;

    public BordspellenAvondService(UserManager<ApplicationUser> userManager, ISpellenRepository repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<bool> IsUserEligibleToOrganizeAsync(ApplicationUser user)
    {
        if (!user.Geboortedatum.HasValue) return false;

        var today = DateTime.Today;
        var geboortedatum = user.Geboortedatum.Value; 

        var age = today.Year - geboortedatum.Year;
        if (geboortedatum > today.AddYears(-age)) age--;

        return age >= 18;
    }


    public async Task CreateBordspellenAvondAsync(BordspellenAvond avond, List<int> geselecteerdeBordspellenIds)
    {
        avond.Bordspellen = await _repository.GetBordspellenByIdsAsync(geselecteerdeBordspellenIds);
        await _repository.AddBordspellenAvondAsync(avond);
    }

    public async Task<List<BordspellenAvond>> GetAllAvondenAsync()
    {
        return await _repository.GetAllBordspellenAvondenAsync();
    }

    public async Task<List<Bordspel>> GetAllBordspellenAsync()
    {
        return await _repository.GetAllBordspellenAsync();
    }

    public async Task<BordspellenAvond> GetAvondByIdAsync(int id)
    {
        var avond = await _repository.GetBordspellenAvondByIdAsync(id);

        // Haal organisator-informatie op uit AspNetUsers als OrganisatorId bestaat
        if (avond != null && !string.IsNullOrEmpty(avond.OrganisatorId))
        {
            avond.Organisator = await _userManager.FindByIdAsync(avond.OrganisatorId);
        }

        return avond;
    }

    public async Task<bool> UserCanEditOrDeleteAsync(int avondId, string userId)
    {
        var avond = await _repository.GetBordspellenAvondByIdAsync(avondId);
        return avond.OrganisatorId == userId && !avond.Inschrijvingen.Any();
    }

    public async Task UpdateBordspellenAvondAsync(BordspellenAvond avond, List<int> geselecteerdeBordspellenIds)
    {
        avond.Bordspellen = await _repository.GetBordspellenByIdsAsync(geselecteerdeBordspellenIds);
        await _repository.UpdateBordspellenAvondAsync(avond);
    }

    public async Task DeleteBordspellenAvondAsync(int id)
    {
        await _repository.DeleteBordspellenAvondAsync(id);
    }

    public async Task<Bordspel> GetBordspelByIdAsync(int id)
    {
        return await _repository.GetBordspelByIdAsync(id);
    }

    public async Task<BordspellenAvond> GetAvondWithInschrijvingenAsync(int id)
    {
        return await _repository.GetAvondWithInschrijvingenAsync(id);
    }

    public async Task<List<BordspellenAvond>> GetAvondenByOrganisatorAsync(string organisatorId)
    {
        return await _repository.GetAvondenByOrganisatorAsync(organisatorId);
    }

    public async Task<List<BordspellenAvond>> GetAvondenWaarIngeschrevenAsync(string userId)
    {
        var avonden = await _repository.GetAvondenWaarIngeschrevenAsync(userId);
        return avonden;
    }

    public async Task<double> BerekenGemiddeldeScoreOrganisatorAsync(string organisatorId)
    {
        return await _repository.BerekenGemiddeldeScoreOrganisatorAsync(organisatorId);
    }

    public async Task AddReviewAsync(Review review)
    {
        await _repository.AddReviewAsync(review);
    }
    public async Task<List<Review>> GetReviewsByOrganisatorAsync(string organisatorId)
    {
        var avonden = await _repository.GetAvondenByOrganisatorAsync(organisatorId);

        var reviews = avonden
            .SelectMany(a => a.Reviews)
            .ToList();

        foreach (var review in reviews)
        {
            review.Speler = await _userManager.FindByIdAsync(review.SpelerId);
        }

        return reviews;
    }

    public async Task<bool> ValidateBordspellenAvond(BordspellenAvond model, List<int> geselecteerdeBordspellen)
    {
        var geselecteerdeSpellen = await _repository.GetBordspellenByIdsAsync(geselecteerdeBordspellen);

        if (!model.Is18Plus && geselecteerdeSpellen.Any(s => s.Is18Plus))
        {
            return false; 
        }

        return true; 
    }


}
