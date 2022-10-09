using Microsoft.EntityFrameworkCore;
using Models;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> optins) : base(optins) { }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Flat> Flats { get; set; }
}