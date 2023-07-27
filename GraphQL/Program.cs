using ConferencePlanner.GraphQL;
using ConferencePlanner.GraphQL.Data;
using HotChocolate.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddPooledDbContextFactory<ApplicationDbContext>(options => options.UseSqlite("Data Source=conferences.db"))
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
    .AddMutationType<Mutation>();
    

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseRouting();
app.MapGraphQL();
app.UseDeveloperExceptionPage();

app.Run();