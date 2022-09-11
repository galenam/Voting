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

    public async Task AddOwners(IEnumerable<OwnerDTO> owners)
    {

    }
}