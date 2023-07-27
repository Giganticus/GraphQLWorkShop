using System.Collections.Generic;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL;

public class Query
{
    public Task<List<Speaker>> GetSpeakers(ApplicationDbContext context) => context.Speakers.ToListAsync();
}