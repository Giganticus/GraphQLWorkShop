using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Speakers;

[ExtendObjectType("Query")]  //If you don't have this attribute the available queries show up in their own type in the SD.
public class SpeakerQueries
{
    public Task<List<Speaker>> GetSpeakersAsync(ApplicationDbContext context)
    {
        return context.Speakers.ToListAsync();
    }

    public Task<Speaker> GetSpeakerByIdAsync(
        [ID(nameof(Speaker))] int id, //this annotation is required to show in the SD that the id of the Speak is an ID 
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