using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Sessions;

[ExtendObjectType("Query")]
public class SessionQueries
{
    public async Task<IEnumerable<Session>> GetSessionsAsync(
        ApplicationDbContext context,
        CancellationToken cancellationToken) => await context.Sessions.ToListAsync(cancellationToken);
    
    public async Task<Session> GetSessionByIdAsync(
        [ID(nameof(Session))] int id,
        SessionByIdDataLoader sessionByIdDataLoader,
        CancellationToken cancellationToken) => await sessionByIdDataLoader.LoadAsync(id, cancellationToken);

    public async Task<IEnumerable<Session>> GetSessionsByIdAsync(
        [ID(nameof(Session))] IReadOnlyCollection<int> ids,
        SessionByIdDataLoader sessionByIdDataLoader,
        CancellationToken cancellationToken) => await sessionByIdDataLoader.LoadAsync(ids, cancellationToken);
}