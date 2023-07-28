using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Types;

public class SpeakerType : ObjectType<Speaker>
{
    protected override void Configure(IObjectTypeDescriptor<Speaker> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode((ctx, id) => ctx.DataLoader<SpeakerByIdDataLoader>()
                .LoadAsync(id, ctx.RequestAborted));

        descriptor
            .Field(t => t.SessionSpeakers)
            .ResolveWith<SpeakerResolvers>(t =>
                SpeakerResolvers.GetSessionsAsync(default!, default!, default!, default!))
            .UseDbContext<ApplicationDbContext>()
            .Name("sessions");
    }

    private class SpeakerResolvers
    {
        public static async Task<IEnumerable<Session>> GetSessionsAsync(
            [Parent] Speaker speaker,
            ApplicationDbContext dbContext,
            SessionByIdDataLoader sessionByIdDataLoader,
            CancellationToken cancellationToken)
        {
            var sessionIds = await dbContext.Speakers
                .Where(s => s.Id == speaker.Id)
                .Include(s => s.SessionSpeakers)
                .SelectMany(s => s.SessionSpeakers.Select(t => t.SessionId))
                .ToArrayAsync(cancellationToken);

            return await sessionByIdDataLoader.LoadAsync(sessionIds, cancellationToken);
        }
    }
}