using Models;

namespace Services;

public interface IGetDataFromSourceService
{
    IEnumerable<OwnerData> Get();
}