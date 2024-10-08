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
        var today = DateTime.Today;
        var age = today.Year - user.Geboortedatum.Year;
        if (user.Geboortedatum.Date > today.AddYears(-age)) age--;
        return age >= 18;
    }

    public async Task CreateBordspellenAvondAsync(BordspellenAvond avond, List<int> geselecteerdeBordspellenIds, string organisatorId)
    {
        avond.OrganisatorId = organisatorId;
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
        return await _repository.GetBordspellenAvondByIdAsync(id);
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
}
