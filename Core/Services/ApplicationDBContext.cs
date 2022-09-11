using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> optins) : base(optins) { }
    public DbSet<Owner> Owners { get; set; }
    public async Task AddOwners(IEnumerable<Owner> owners)
    {
        await Owners.AddRangeAsync(owners);
        await SaveChangesAsync();
    }
}