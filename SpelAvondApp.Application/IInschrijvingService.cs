﻿using SpelAvondApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelAvondApp.Application
{
    public interface IInschrijvingService
    {
        Task<bool> InschrijvenVoorAvondAsync(string userId, int avondId, string dieetWensen);
        Task<bool> HeeftAlIngeschreven(string userId, int avondId);
        Task<BordspellenAvond> GetAvondWithInschrijvingenAndUserNamesAsync(int avondId);
        Task<BordspellenAvond> GetAvondMetDieetOptiesAsync(int avondId);
        Task<bool> KanDeelnemenAanAvond(ApplicationUser gebruiker, DateTime avondDatum);
        Task<bool> MagDeelnemenOpBasisVanLeeftijdAsync(string userId, int avondId);
    }
}
