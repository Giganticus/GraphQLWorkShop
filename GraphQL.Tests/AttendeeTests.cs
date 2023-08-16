using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Attendees;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.Types;
using HotChocolate.Data;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;

namespace GraphQL.Tests;

public class AttendeeTests
{
    [Fact]
    public async Task Attendee_Schema_Changed()
    {
        // arrange
        // act
        var schema = await new ServiceCollection()
            .AddPooledDbContextFactory<ApplicationDbContext>(
                options => options.UseInMemoryDatabase("Data Source=conferences.db"))
            .AddGraphQL()
            .AddQueryType(d => d.Name("Query"))
                 .AddTypeExtension<AttendeeQueries>()
            
             .AddMutationType(d => d.Name("Mutation"))
                 .AddTypeExtension<AttendeeMutations>()
             .AddType<AttendeeType>()
             .AddType<SessionType>()
             .AddType<SpeakerType>()
             .AddType<TrackType>()
            .AddGlobalObjectIdentification()
            .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
            .BuildSchemaAsync();
            
        // assert
        var schemaPrint = schema.Print();
        schema.Print().MatchSnapshot();
    }
}