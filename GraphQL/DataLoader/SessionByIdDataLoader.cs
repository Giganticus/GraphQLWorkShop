using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.DataLoader;

public class SessionByIdDataLoader : BatchDataLoader<int, Session>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public SessionByIdDataLoader(
        IBatchScheduler batchScheduler, 
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    {
        _dbContextFactory = dbContextFactory ??
                            throw new ArgumentNullException(nameof(dbContextFactory));
    }

    protected override async Task<IReadOnlyDictionary<int, Session>> LoadBatchAsync(
        IReadOnlyList<int> keys, 
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.Sessions
            .Where(x => keys.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
    }
}