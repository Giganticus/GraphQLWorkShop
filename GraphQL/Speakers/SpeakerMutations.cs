﻿using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using HotChocolate.Types;

namespace ConferencePlanner.GraphQL.Speakers;

[ExtendObjectType("Mutation")]
public class SpeakerMutations
{
    public async Task<AddSpeakerPayload> AddSpeakerAsync(
        AddSpeakerInput input,
        ApplicationDbContext context)
    {
        var speaker = new Speaker
        {
            Name = input.Name,
            Bio = input.Bio,
            WebSite = input.WebSite
        };

        context.Speakers.Add(speaker);

        await context.SaveChangesAsync();

        return new AddSpeakerPayload(speaker);
    }
}