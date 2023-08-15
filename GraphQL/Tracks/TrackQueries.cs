using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Tracks;

[ExtendObjectType("Query")]
public class TrackQueries
{
    [UsePaging]
    public IQueryable<Track> GetTracks(
        ApplicationDbContext context) =>
        context.Tracks.OrderBy(t => t.Name);
    
    // public async Task<IEnumerable<Track>> GetTracksAsync(
    //     ApplicationDbContext context,
    //     CancellationToken cancellationToken) =>
    //     await context.Tracks.ToListAsync(cancellationToken);

    public async Task<Track> GetTrackByNameAsync(
        string name,
        ApplicationDbContext context,
        CancellationToken cancellationToken) =>
        await context.Tracks.FirstAsync(t => t.Name == name, cancellationToken);
    
    public async Task<IEnumerable<Track>> GetTrackByNamesAsync(
        string[] names,
        ApplicationDbContext context,
        CancellationToken cancellationToken) =>
        await context.Tracks.Where(t => names.Contains(t.Name)).ToListAsync(cancellationToken);
    
    public Task<Track> GetTrackByIdAsync(
        [ID(nameof(Track))] int id,
        TrackByIdDataLoader trackById,
        CancellationToken cancellationToken) =>
        trackById.LoadAsync(id, cancellationToken);
    
    
    public async Task<IEnumerable<Track>> GetTracksByIdAsync(
        [ID(nameof(Track))] int[] ids,
        TrackByIdDataLoader trackById,
        CancellationToken cancellationToken) =>
        await trackById.LoadAsync(ids, cancellationToken);
}