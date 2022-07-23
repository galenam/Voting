using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;

namespace Service;

public class FillDataService : IFillDataService
{
    private ILogger<FillDataService> _logger;
    private Settings _settings;
    public FillDataService(ILogger<FillDataService> logger, IOptions<Settings> options)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public void FillDb()
    {
        _logger.LogError(123, "error message");
    }
}