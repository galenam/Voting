using Models;

namespace Services;

public interface IGetDataFromSourceService
{
    IEnumerable<OwnerVoting> Get();
}