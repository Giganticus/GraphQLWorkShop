using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL;

[ExtendObjectType("Query")]
public class SpeakersQueries
{
    public Task<List<Speaker>> GetSpeakers(ApplicationDbContext context)
    {
        return context.Speakers.ToListAsync();
    }

    public Task<Speaker> GetSpeakerAsync(
        [ID(nameof(Speaker))] int id, //not sure this ID annotation is needed
        SpeakerByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        return dataLoader.LoadAsync(id, cancellationToken);
    }

    public Task<IReadOnlyList<Speaker>> GetSpeakersByIdsAsync(
        IReadOnlyCollection<int> ids,
        SpeakerByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        return dataLoader.LoadAsync(ids, cancellationToken);
    }
}