using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;

namespace Services;

public class FillDataService : IFillDataService
{
    private readonly ILogger<FillDataService> _logger;
    private readonly Settings _settings;

    private readonly IGetDataFromSourceService _dataService;
    public FillDataService(ILogger<FillDataService> logger, IOptions<Settings> options, IGetDataFromSourceService dataService)
    {
        _logger = logger;
        _settings = options.Value;
        _dataService = dataService;
    }

    public async Task<bool> FillDb()
    {
        var data = _dataService.Get();

        return true;
    }
}