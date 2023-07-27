using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL;

public class Query
{
    public Task<List<Speaker>> GetSpeakers(ApplicationDbContext context) => context.Speakers.ToListAsync();

    public Task<Speaker> GetSpeakerAsync(
        int id,
        SpeakerByIdDataLoader dataLoader,
        CancellationToken cancellationToken) => dataLoader.LoadAsync(id, cancellationToken);

    public Task<IReadOnlyList<Speaker>> GetSpeakersByIdsAsync(
        IReadOnlyCollection<int> ids,
        SpeakerByIdDataLoader dataLoader,
        CancellationToken cancellationToken) => dataLoader.LoadAsync(ids, cancellationToken);
}