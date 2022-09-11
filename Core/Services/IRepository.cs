using Models;

namespace Services;

public interface IRepository
{
    Task AddOwners(IEnumerable<OwnerDTO> owners);
}