﻿using SpelAvondApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISpellenRepository
{
    // Bordspel Methods
    Task AddBordspelAsync(Bordspel bordspel);
    Task UpdateBordspelAsync(Bordspel bordspel);
    Task DeleteBordspelAsync(int id);
    Task<Bordspel> GetBordspelByIdAsync(int id);
    Task<List<Bordspel>> GetAllBordspellenAsync();

    // BordspellenAvond Methods
    Task AddBordspellenAvondAsync(BordspellenAvond avond);
    Task UpdateBordspellenAvondAsync(BordspellenAvond avond);
    Task DeleteBordspellenAvondAsync(int id);
    Task<BordspellenAvond> GetBordspellenAvondByIdAsync(int id);
    Task<List<BordspellenAvond>> GetAllBordspellenAvondenAsync();

    Task<List<Bordspel>> GetBordspellenByIdsAsync(List<int> ids);
}
