using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;

namespace Services;

public class FillDataService : IFillDataService
{
    private readonly ILogger<FillDataService> _logger;
    private readonly Settings _settings;

    private readonly IGetDataFromSourceService _dataService;
    private readonly IRepository _repo;
    public FillDataService(ILogger<FillDataService> logger, IOptions<Settings> options, IGetDataFromSourceService dataService,
        IRepository repo)
    {
        _logger = logger;
        _settings = options.Value;
        _dataService = dataService;
        _repo = repo;
    }

    public async Task<bool> FillDb()
    {
        var data = _dataService.Get();
        var owners = data.Select(d => new Owner { Name = d.Name });
        await _repo.AddOwners(owners);

        return true;
    }
}