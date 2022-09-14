using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Models;

public class ApplicationDBContext : DbContext
{
    //public ApplicationDBContext(IOptions<Settings> options) { }
    //protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseNpgsql()
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> optins) : base(optins) { }
    public DbSet<Owner> Owners { get; set; }
}