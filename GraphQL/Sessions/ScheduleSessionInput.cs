using System;
using HotChocolate.Types.Relay;

namespace ConferencePlanner.GraphQL.Sessions;

public record ScheduleSessionInput(
    [property: ID] int SessionId,
    [property: ID] int TrackId,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);