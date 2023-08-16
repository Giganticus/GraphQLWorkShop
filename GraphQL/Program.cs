using ConferencePlanner.GraphQL.Attendees;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using ConferencePlanner.GraphQL.Sessions;
using ConferencePlanner.GraphQL.Speakers;
using ConferencePlanner.GraphQL.Tracks;
using ConferencePlanner.GraphQL.Types;
using HotChocolate.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddPooledDbContextFactory<ApplicationDbContext>(options => options.UseSqlite("Data Source=conferences.db"))
    .AddGraphQLServer()
    .AddQueryType(d =>
    {
        d.Name("Query");
        d.Description("QueryDescription");
    })
        .AddTypeExtension<AttendeeQueries>()
        .AddTypeExtension<SpeakerQueries>()
        .AddTypeExtension<SessionQueries>()
        .AddTypeExtension<TrackQueries>()
    .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
    .AddMutationType(x => x.Name("Mutation"))
        .AddTypeExtension<AttendeeMutations>()
        .AddTypeExtension<SpeakerMutations>()
        .AddTypeExtension<SessionMutations>()
        .AddTypeExtension<TrackMutations>()
    .AddSubscriptionType(d => d.Name("Subscription"))
        .AddTypeExtension<SessionSubscriptions>()
        .AddTypeExtension<AttendeeSubscriptions>()
    .AddType<SpeakerType>()
    .AddType<AttendeeType>()
    .AddType<TrackType>()
    .AddType<SessionType>()
    .AddFiltering()
    .AddSorting()
    .AddInMemorySubscriptions() 
    .AddGlobalObjectIdentification()
    .AddDataLoader<SpeakerByIdDataLoader>()
    .AddDataLoader<SessionByIdDataLoader>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseWebSockets();
app.UseRouting();
app.MapGraphQL();
app.UseDeveloperExceptionPage();


app.Run();