using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Attendees;

[ExtendObjectType(Name = "Mutation")]
public class AttendeeMutations
{
    public async Task<RegisterAttendeePayload> RegisterAttendeeAsync(
        RegisterAttendeeInput input,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var attendee = new Attendee
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            UserName = input.UserName,
            EmailAddress = input.EmailAddress
        };

        context.Attendees.Add(attendee);

        await context.SaveChangesAsync(cancellationToken);

        return new RegisterAttendeePayload(attendee);
    }
    
    public async Task<CheckInAttendeePayload> CheckInAttendeeAsync(
        CheckInAttendeeInput input,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var attendee = (await context.Attendees.FirstOrDefaultAsync(
            t => t.Id == input.AttendeeId, cancellationToken))!;

        attendee.SessionsAttendees.Add(
            new SessionAttendee
            {
                SessionId = input.SessionId
            });

        await context.SaveChangesAsync(cancellationToken);

        return new CheckInAttendeePayload(attendee, input.SessionId);
    }
}