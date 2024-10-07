using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SpelAvondApp.Domain.Models;

public class BordspelService : IBordspelService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISpellenRepository _repository;

    public BordspelService(UserManager<ApplicationUser> userManager, ISpellenRepository repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<bool> IsUserEligibleToCreateBordspel(ApplicationUser user)
    {
        var today = DateTime.Today;
        var age = today.Year - user.Geboortedatum.Year;
        if (user.Geboortedatum.Date > today.AddYears(-age)) age--;
        return age >= 18;
    }

    public async Task AddBordspelAsync(Bordspel bordspel)
    {
        await _repository.AddBordspelAsync(bordspel);
    }

    public async Task UpdateBordspelAsync(Bordspel bordspel)
    {
        await _repository.UpdateBordspelAsync(bordspel);
    }

    public async Task DeleteBordspelAsync(int id)
    {
        await _repository.DeleteBordspelAsync(id);
    }

    public async Task<Bordspel> GetBordspelByIdAsync(int id)
    {
        return await _repository.GetBordspelByIdAsync(id);
    }

    public async Task<List<Bordspel>> GetAllBordspellenAsync()
    {
        return await _repository.GetAllBordspellenAsync();
    }
}
