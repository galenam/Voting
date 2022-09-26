using Models;

namespace Services;

public interface IGetDataFromSourceService
{
    IEnumerable<OwnerDataDTO> Get();
}