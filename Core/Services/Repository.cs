using Mapster;
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

    public async Task<bool> AddOwner(OwnerData ownerData)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        using var context = new ApplicationDBContext(scope.ServiceProvider.GetRequiredService<DbContextOptions<ApplicationDBContext>>());

        var result = false;
        try
        {
            var owner = ownerData.Adapt<Owner>();
<<<<<<< HEAD
            //owner.Flat = ownerData.Adapt<OwnerFlat>();
            //await context.Owners.AddAsync(ownerData.Adapt<Owner>());
            //await context.SaveChangesAsync();
=======
            owner.Flat = ownerData.Adapt<OwnerFlat>();
            await context.Owners.AddAsync(ownerData.Adapt<Owner>());
            result = true;
>>>>>>> 26620bbd209e7b8035fa31f2823442b3a822a169
        }
        catch (DbUpdateException ex)
        {
            _logger.LogInformation(ex, $"Owner exists {ownerData.Name}");
        }
<<<<<<< HEAD
=======
        await context.SaveChangesAsync();
        return result;
>>>>>>> 26620bbd209e7b8035fa31f2823442b3a822a169
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