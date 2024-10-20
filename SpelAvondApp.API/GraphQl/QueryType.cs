using HotChocolate;
using SpelAvondApp.Domain.Models;

public class QueryType
{
    public async Task<List<BordspellenAvond>> GetBordspellenAvonden([Service] ISpellenRepository repository)
    {
        return await repository.GetAllBordspellenAvondenAsync();
    }

    public async Task<List<Bordspel>> GetBordspellen([Service] ISpellenRepository repository)
    {
        return await repository.GetAllBordspellenAsync();
    }

    public async Task<IEnumerable<Review>> GetOrganisatorReviews(string organisatorId, [Service] ISpellenRepository repository)
    {
        var avonden = await repository.GetAvondenByOrganisatorAsync(organisatorId);
        return avonden.SelectMany(avond => avond.Reviews);
    }
}
