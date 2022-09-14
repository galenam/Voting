using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace Services;

public class Repository : IRepository
{
    IServiceProvider _serviceProvider;
    IServiceScopeFactory _serviceScopeFactory;
    public Repository(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
    {
        _serviceProvider = serviceProvider;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task AddOwners(IEnumerable<Owner> owners)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        using var context = new ApplicationDBContext(scope.ServiceProvider.GetRequiredService<DbContextOptions<ApplicationDBContext>>());
        await context.Owners.AddRangeAsync(owners);
        await context.SaveChangesAsync();
    }
}