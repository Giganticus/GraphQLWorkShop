using HotChocolate.Types.Relay;

namespace ConferencePlanner.GraphQL.Attendees;

public record CheckInAttendeeInput(
    //[ID(nameof(Session))]
    [property: ID] int SessionId,
    //[ID(nameof(Attendee))]
    [property: ID]int AttendeeId);