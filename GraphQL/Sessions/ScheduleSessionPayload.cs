using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Common;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;

namespace ConferencePlanner.GraphQL.Sessions;

public class ScheduleSessionPayload : SessionPayloadBase
{
    public ScheduleSessionPayload(UserError error)
        : base(new[] { error })
    {
    }
    
    public ScheduleSessionPayload(Session session) : base(session)
    {
    }

    public ScheduleSessionPayload(IReadOnlyList<UserError> errors) : base(errors)
    {
    }
    
    public async Task<Track?> GetTrackAsync(
        TrackByIdDataLoader trackById,
        CancellationToken cancellationToken)
    {
        if (Session is null)
        {
            return null;
        }

        return await trackById.LoadAsync(Session.Id, cancellationToken);
    }
    
    public async Task<IEnumerable<Speaker>?> GetSpeakersAsync(
        ApplicationDbContext dbContext,
        SpeakerByIdDataLoader speakerById,
        CancellationToken cancellationToken)
    {
        if (Session is null)
        {
            return null;
        }

        var speakerIds = await dbContext.Sessions
            .Where(s => s.Id == Session.Id)
            .Include(s => s.SessionSpeakers)
            .SelectMany(s => s.SessionSpeakers.Select(t => t.SpeakerId))
            .ToArrayAsync(cancellationToken: cancellationToken);

        return await speakerById.LoadAsync(speakerIds, cancellationToken);
    }
}