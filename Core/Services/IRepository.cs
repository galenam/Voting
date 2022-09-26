using Models;

namespace Services;

public interface IRepository
{
    Task AddOwner(OwnerData ownerData);
    Task<bool> IsOwnerExist(string ownerName);
}