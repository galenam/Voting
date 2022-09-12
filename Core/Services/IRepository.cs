using Models;

namespace Services;

public interface IRepository
{
    Task AddOwners(IEnumerable<Owner> owners);
}