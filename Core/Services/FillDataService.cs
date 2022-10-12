using System.ComponentModel.DataAnnotations;
using Mapster;
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

        var groups = data
            .Where(d =>
            {
                var vc = new ValidationContext(d);
                var errorResults = new List<ValidationResult>();
                return Validator.TryValidateObject(d, vc, errorResults);
            }).GroupBy(d => d.FlatNumber);

        foreach (var group in groups)
        {
            var ownerWithCorrectFlat = group.Where(g => g.FlatSquare > 0).First();
            var ownerData = ownerWithCorrectFlat.Adapt<OwnerData>();
            ownerData.FlatId = await _repo.AddFlat(ownerData);
        }
/*
        foreach (var owner in owners)
        {
            var ownerData = owner.Adapt<OwnerData>();
            ownerData.FlatId = await _repo.AddFlat(ownerData);
            await _repo.AddOwner(ownerData);
        }
        */
        return true;
    }
}