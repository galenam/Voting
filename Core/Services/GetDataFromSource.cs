using System.Drawing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
namespace Services;
public class GetDataFromSourceService : IGetDataFromSourceService
{
    private ILogger<GetDataFromSourceService> _logger;
    private Settings _settings;
    public GetDataFromSourceService(ILogger<GetDataFromSourceService> logger, IOptions<Settings> options)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public async Task<IEnumerable<OwnerVoting>> Get()
    {

        return null;
    }
}