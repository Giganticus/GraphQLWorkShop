using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Types;

public class AttendeeType : ObjectType<Attendee>
{
    protected override void Configure(IObjectTypeDescriptor<Attendee> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(x => x.Id)
            .ResolveNode((ctx, id) => ctx.DataLoader<AttendeeByIdDataLoader>()
                .LoadAsync(id, ctx.RequestAborted));

        descriptor
            .Field(t => t.SessionsAttendees)
            .ResolveWith<AttendeeResolvers>(x => x.GetSessionsAsync(default!, default!, default!, default));
    }

    private class AttendeeResolvers
    {
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            Attendee attendee,
            ApplicationDbContext dbContext,
            SessionByIdDataLoader sessionByIdDataLoader,
            CancellationToken cancellationToken)
        {
            var speakerIds = await dbContext.Attendees
                .Where(x => x.Id == attendee.Id)
                .Include(x => x.SessionsAttendees)
                .SelectMany(x => x.SessionsAttendees).Select(x => x.SessionId)
                .ToArrayAsync(cancellationToken);

            return await sessionByIdDataLoader.LoadAsync(speakerIds, cancellationToken);
        }
    }
}