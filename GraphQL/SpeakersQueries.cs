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
public class SpeakerQueries
{
    public Task<List<Speaker>> GetSpeakersAsync(ApplicationDbContext context)
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

    public Task<IReadOnlyList<Speaker>> GetSpeakersByIdAsync(
        [ID(nameof(Speaker))] IReadOnlyCollection<int> ids,
        SpeakerByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        return dataLoader.LoadAsync(ids, cancellationToken);
    }
}