using System.Collections.Generic;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using HotChocolate.Types;

namespace ConferencePlanner.GraphQL.Types;

public class SpeakerType : ObjectType<Speaker>
{
    protected override void Configure(IObjectTypeDescriptor<Speaker> descriptor)
    {
        descriptor
            .Field(t => t.SessionSpeakers)
            .ResolveWith<SpeakerResolvers>(t => t.Get)
    }

    private class SpeakerResolvers
    {
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            Speaker speaker,
            ApplicationDbContext dbContext,
            )
        {
            
        }
    }
}