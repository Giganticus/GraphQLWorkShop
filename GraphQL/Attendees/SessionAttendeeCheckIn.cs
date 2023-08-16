using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Attendees;

public class SessionAttendeeCheckIn
{
    public SessionAttendeeCheckIn(int attendeeId, int sessionId)
    {
        AttendeeId = attendeeId;
        SessionId = sessionId;
    }

    //[ID(nameof(Attendee))]
    [property: ID]
    public int AttendeeId { get; }

    //[ID(nameof(Session))]
    [property: ID]
    public int SessionId { get; }
    
    public async Task<int> CheckInCountAsync(
        ApplicationDbContext context,
        CancellationToken cancellationToken) =>
        await context.Sessions
            .Where(session => session.Id == SessionId)
            .SelectMany(session => session.SessionAttendees)
            .CountAsync(cancellationToken);

    public Task<Attendee> GetAttendeeAsync(
        AttendeeByIdDataLoader attendeeById,
        CancellationToken cancellationToken) =>
        attendeeById.LoadAsync(AttendeeId, cancellationToken);

    public Task<Session> GetSessionAsync(
        SessionByIdDataLoader sessionById,
        CancellationToken cancellationToken) =>
        sessionById.LoadAsync(SessionId, cancellationToken);   
}