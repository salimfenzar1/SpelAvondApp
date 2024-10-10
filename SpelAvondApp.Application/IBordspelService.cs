using SpelAvondApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBordspelService
{
    Task<bool> IsUserEligibleToCreateBordspel(ApplicationUser user);
    Task AddBordspelAsync(Bordspel bordspel);
    Task UpdateBordspelAsync(Bordspel bordspel);
    Task DeleteBordspelAsync(int id);
    Task<Bordspel> GetBordspelByIdAsync(int id);
    Task<List<Bordspel>> GetAllBordspellenAsync();
}
