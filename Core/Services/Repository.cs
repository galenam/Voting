using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;

namespace Services;

public class Repository : IRepository
{
    IServiceScopeFactory _serviceScopeFactory;
    ILogger<Repository> _logger;

    public Repository(ILogger<Repository> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task AddOwner(Owner owner)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        using var context = new ApplicationDBContext(scope.ServiceProvider.GetRequiredService<DbContextOptions<ApplicationDBContext>>());

        try
        {
            await context.Owners.AddAsync(owner);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogInformation(ex, $"Owner exists {owner.Name}");
        }
        await context.SaveChangesAsync();

    }

    public async Task<bool> IsOwnerExist(string ownerName)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        using var context = new ApplicationDBContext(scope.ServiceProvider.GetRequiredService<DbContextOptions<ApplicationDBContext>>());

        try
        {
            return await context.Owners.FirstOrDefaultAsync(o => o.Name.ToLower() == ownerName.ToLower()) != null;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogInformation(ex, $"Owner get error {ownerName}");
        }
        return true;
    }
}