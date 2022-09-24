using System.ComponentModel.DataAnnotations;
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
        var owners = data
            .Select(d => new Owner { Name = d.Name })
            .Where(d =>
            {
                var vc = new ValidationContext(d);
                var errorResults = new List<ValidationResult>();
                return Validator.TryValidateObject(d, vc, errorResults);
            });
        foreach (var owner in owners)
        {
            var isOwnerExist = await _repo.IsOwnerExist(owner.Name);
            if (!isOwnerExist)
            {
                await _repo.AddOwner(owner);
            }
        }
        return true;
    }
}