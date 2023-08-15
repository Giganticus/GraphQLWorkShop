using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using ConferencePlanner.GraphQL.Types;
using HotChocolate.Data;
using HotChocolate.Data.Filters;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace ConferencePlanner.GraphQL.Sessions;

[ExtendObjectType("Query")]
public class SessionQueries
{
    [UsePaging(typeof(NonNullType<SessionType>))]
    [UseFiltering(typeof(SessionFilterInputType))]
    [UseSorting]
    public IQueryable<Session> GetSessions(
        ApplicationDbContext context) =>
        context.Sessions;
    
    // public async Task<IEnumerable<Session>> GetSessionsAsync(
    //     ApplicationDbContext context,
    //     CancellationToken cancellationToken) => await context.Sessions.ToListAsync(cancellationToken);
    
    public async Task<Session> GetSessionByIdAsync(
        [ID(nameof(Session))] int id,
        SessionByIdDataLoader sessionByIdDataLoader,
        CancellationToken cancellationToken) => await sessionByIdDataLoader.LoadAsync(id, cancellationToken);

    public async Task<IEnumerable<Session>> GetSessionsByIdAsync(
        [ID(nameof(Session))] IReadOnlyCollection<int> ids,
        SessionByIdDataLoader sessionByIdDataLoader,
        CancellationToken cancellationToken) => await sessionByIdDataLoader.LoadAsync(ids, cancellationToken);
}

public class SessionFilterInputType : FilterInputType<Session>
{
    protected override void Configure(IFilterInputTypeDescriptor<Session> descriptor)
    {
        descriptor.Ignore(t => t.Id);
        descriptor.Ignore(t => t.TrackId);
    }
}