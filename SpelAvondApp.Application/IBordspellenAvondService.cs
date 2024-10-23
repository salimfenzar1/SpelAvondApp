using SpelAvondApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBordspellenAvondService
{
    Task<bool> IsUserEligibleToOrganizeAsync(ApplicationUser user);
    Task CreateBordspellenAvondAsync(BordspellenAvond avond, List<int> geselecteerdeBordspellen);
    Task UpdateBordspellenAvondAsync(BordspellenAvond avond, List<int> geselecteerdeBordspellen);

    Task<List<BordspellenAvond>> GetAllAvondenAsync();
    Task<List<Bordspel>> GetAllBordspellenAsync();
    Task<BordspellenAvond> GetAvondByIdAsync(int id);
    Task<bool> UserCanEditOrDeleteAsync(int avondId, string userId);
    Task DeleteBordspellenAvondAsync(int id);
    Task<Bordspel> GetBordspelByIdAsync(int id);
    Task<BordspellenAvond> GetAvondWithInschrijvingenAsync(int id);

    Task<List<BordspellenAvond>> GetAvondenByOrganisatorAsync(string organisatorId);
    public Task<List<BordspellenAvond>> GetAvondenWaarIngeschrevenAsync(string userId);

    Task<double> BerekenGemiddeldeScoreOrganisatorAsync(string organisatorId);
    Task AddReviewAsync(Review review);

    Task<List<Review>> GetReviewsByOrganisatorAsync(string organisatorId);
    Task<bool> ValidateBordspellenAvond(BordspellenAvond model, List<int> geselecteerdeBordspellen);

}
