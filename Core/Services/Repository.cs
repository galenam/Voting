using Microsoft.EntityFrameworkCore;
using Models;

namespace Services;

public class Repository : IRepository
{
    IDbContextFactory<ApplicationDBContext> _contextFactory;
    public Repository(IDbContextFactory<ApplicationDBContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task AddOwners(IEnumerable<Owner> owners)
    {
        using var context = _contextFactory.CreateDbContextAsync();
        await (await context).Owners.AddRangeAsync(owners);
    }
}