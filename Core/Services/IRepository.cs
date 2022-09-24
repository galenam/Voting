using Models;

namespace Services;

public interface IRepository
{
    Task AddOwner(Owner owner);
    Task<bool> IsOwnerExist(string ownerName);
}