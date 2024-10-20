using SpelAvondApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBordspellenAvondService
{
    // Bestaande methoden
    Task<bool> IsUserEligibleToOrganizeAsync(ApplicationUser user);

    // Pas deze methoden aan voor ondersteuning van meerdere bordspellen
    Task CreateBordspellenAvondAsync(BordspellenAvond avond, List<int> geselecteerdeBordspellen);
    Task UpdateBordspellenAvondAsync(BordspellenAvond avond, List<int> geselecteerdeBordspellen);

    Task<List<BordspellenAvond>> GetAllAvondenAsync();
    Task<List<Bordspel>> GetAllBordspellenAsync();
    Task<BordspellenAvond> GetAvondByIdAsync(int id);
    Task<bool> UserCanEditOrDeleteAsync(int avondId, string userId);
    Task DeleteBordspellenAvondAsync(int id);

    // Voeg deze methode toe om een bordspel op ID op te halen
    Task<Bordspel> GetBordspelByIdAsync(int id);
    Task<BordspellenAvond> GetAvondWithInschrijvingenAsync(int id);

    Task<List<BordspellenAvond>> GetAvondenByOrganisatorAsync(string organisatorId);
    public Task<List<BordspellenAvond>> GetAvondenWaarIngeschrevenAsync(string userId);

    Task<double> BerekenGemiddeldeScoreOrganisatorAsync(string organisatorId);
    Task AddReviewAsync(Review review);

    Task<List<Review>> GetReviewsByOrganisatorAsync(string organisatorId);

}
