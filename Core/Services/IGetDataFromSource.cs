using Models;

namespace Services;

public interface IGetDataFromSourceService
{
    Task<IEnumerable<OwnerVoting>> Get();
}