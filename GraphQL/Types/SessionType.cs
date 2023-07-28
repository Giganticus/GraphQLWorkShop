using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Types;

public class SessionType : ObjectType<Session>
{
    protected override void Configure(IObjectTypeDescriptor<Session> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(x => x.Id)
            .ResolveNode((ctx, id) => ctx.DataLoader<SessionByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

        descriptor
            .Field(t => t.SessionSpeakers)
            .ResolveWith<SessionResolvers>(x => x.GetSpeakersAsync(default!, default!, default!, default!))
            .UseDbContext<ApplicationDbContext>()
            .Name("speakers");

        descriptor
            .Field(t => t.SessionAttendees)
            .ResolveWith<SessionResolvers>(x => x.GetAttendeesAsync(default!, default!, default!, default!))
            .UseDbContext<ApplicationDbContext>()
            .Name("attendees");

        descriptor
            .Field(t => t.Track)
            .ResolveWith<SessionResolvers>(t => t.GetTrackAsync(default!, default!, default!));

        descriptor
            .Field(t => t.TrackId)
            .ID(nameof(Track));
    }

    private class SessionResolvers
    {
        public async Task<IEnumerable<Speaker>> GetSpeakersAsync(
            Session session,
            ApplicationDbContext dbContext,
            SpeakerByIdDataLoader speakerByIdDataLoader,
            CancellationToken cancellationToken)
        {
            var speakerIds = await dbContext.Sessions
                .Where(s => s.Id == session.Id)
                .Include(s => s.SessionSpeakers)
                .SelectMany(s => s.SessionSpeakers.Select(x => x.SpeakerId))
                .ToArrayAsync(cancellationToken);

            return await speakerByIdDataLoader.LoadAsync(speakerIds, cancellationToken);
        }

        public async Task<IEnumerable<Attendee>> GetAttendeesAsync(
            Session session,
            ApplicationDbContext dbContext,
            AttendeeByIdDataLoader attendeeByIdDataLoader,
            CancellationToken cancellationToken)
        {
            var attendeeIds = await dbContext.Sessions
                .Where(x => x.Id == session.Id)
                .Include(x => x.SessionAttendees)
                .SelectMany(x => x.SessionAttendees.Select(x => x.AttendeeId))
                .ToArrayAsync(cancellationToken);

            return await attendeeByIdDataLoader.LoadAsync(attendeeIds, cancellationToken);
        }

        public async Task<Track?> GetTrackAsync(
            Session session,
            TrackByIdDataLoader trackByIdDataLoader,
            CancellationToken cancellationToken)
        {
            if (session.TrackId is null) return null;

            return await trackByIdDataLoader.LoadAsync(session.TrackId.Value, cancellationToken);
        }
    }
}