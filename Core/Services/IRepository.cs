using Models;

namespace Services;

public interface IRepository
{
    Task<bool> AddOwner(OwnerData ownerData);
    Task<bool> IsOwnerExist(string ownerName);
}