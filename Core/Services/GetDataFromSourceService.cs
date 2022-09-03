using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Models;
using Tesseract;

namespace Services;
public class GetDataFromSourceService : IGetDataFromSourceService, IDisposable
{
    private ILogger<GetDataFromSourceService> _logger;
    private Settings _settings;

    private readonly TesseractEngine _engine;

    public GetDataFromSourceService(ILogger<GetDataFromSourceService> logger, IOptions<Settings> options,
        TesseractEngine engine)
    {
        _logger = logger;
        _settings = options.Value;
        _engine = engine;
    }

    public void Dispose() => _engine.Dispose();

    public IEnumerable<OwnerData> Get()
    {
        var ownerData = new List<OwnerData>();
        try
        {
            var files = GetFileNames();
            if (!files.Any())
            {
                return Enumerable.Empty<OwnerData>();
            }
            foreach (var file in files)
            {
                try
                {
                    ownerData.AddRange(GetOwnerData(file));
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "OCR error");
                    Console.WriteLine("Unexpected Error: " + e.Message);
                    Console.WriteLine("Details: ");
                    Console.WriteLine(e.ToString());
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("File error: ", e);
        }

        return ownerData;
    }

    internal IList<OwnerData> GetOwnerDataFromStrings(IList<IList<string>> listOfList)
    {
        int indexFlatNumber = 0, indexSquareFlat = 0, indexLivingQuater = 0, indexFlatType = 0, indexOwnerName = 0,
                indexSquareOfPart = 0, indexPercentOfTheWholeHouse = 0;
        var owners = new List<OwnerData>();
        do
        {
            var ownerData = new OwnerData();
            if (indexFlatNumber < listOfList[0].Count)
            {
                var flatNumberString = listOfList[0][indexFlatNumber].CleanIntString();
                if (int.TryParse(flatNumberString, out int flatNumber))
                {
                    ownerData.FlatSquare = flatNumber;
                }
                indexFlatNumber++;
            }
            if (indexSquareFlat < listOfList[1].Count)
            {
                var squareFlatString = listOfList[1][indexSquareFlat].CleanDecimalString();
                if (decimal.TryParse(squareFlatString, out decimal squareFlat))
                {
                    ownerData.FlatSquare = squareFlat;
                }
                indexSquareFlat++;
            }
            if (indexLivingQuater < listOfList[2].Count)
            {
                var livingQuater = listOfList[2][indexLivingQuater];
                ownerData.LivingQuaterType = livingQuater.GetEnumValueByDisplayName<LivingQuater>();
                indexLivingQuater++;
            }
            if (indexFlatType < listOfList[3].Count)
            {
                var flatTypeString = listOfList[3][indexFlatType];
                ownerData.TypeOfFlat = flatTypeString.GetEnumValueByDisplayName<FlatType>();
                indexFlatType++;
            }
            if (indexOwnerName < listOfList[4].Count)
            {
                var ownerName = listOfList[4][indexOwnerName].CleanString();
                ownerData.Name = ownerName;
                indexOwnerName++;
            }
            if (indexSquareOfPart < listOfList[5].Count)
            {
                var squareOfPartString = listOfList[5][indexSquareOfPart].CleanDecimalString();
                if (decimal.TryParse(squareOfPartString, out decimal squareOfPart))
                {
                    ownerData.SquareOfPart = squareOfPart;
                }
                indexSquareOfPart++;
            }
            if (indexPercentOfTheWholeHouse < listOfList[6].Count)
            {
                var percentOfTheWholeHouseString = listOfList[6][indexPercentOfTheWholeHouse].CleanDecimalString();
                if (decimal.TryParse(percentOfTheWholeHouseString, out decimal percentOfTheWholeHouse))
                {
                    ownerData.PercentOfTheWholeHouse = percentOfTheWholeHouse;
                }
                indexPercentOfTheWholeHouse++;
            }

            var vc = new ValidationContext(ownerData);
            var errorResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(ownerData, vc, errorResults);

            if (isValid)
            {
                owners.Add(ownerData);
            }
        }
        while (indexFlatNumber < listOfList[0].Count || indexSquareFlat < listOfList[1].Count
            || indexLivingQuater < listOfList[2].Count || indexFlatType < listOfList[3].Count
            || indexOwnerName < listOfList[4].Count || indexSquareOfPart < listOfList[5].Count
            || indexPercentOfTheWholeHouse < listOfList[6].Count);
        return owners;
    }

    internal IList<OwnerData> GetOwnerData(string file)
    {
        IList<OwnerData> owners = new List<OwnerData>();

        var listOfList = new List<IList<string>>();
        using var img = Pix.LoadFromFile(file);
        _logger.LogDebug($"File name: {file}");
        foreach (var c in _settings.Coordinates)
        {
            var rect = c.GetRectFromCoordinates();
            using (var page = _engine.Process(img, rect, PageSegMode.SingleColumn))
            {
                _logger.LogDebug($"Coordinates: {c}, Whole text: {page.GetText()}");
                _logger.LogDebug("Text (iterator):");
                using (var iter = page.GetIterator())
                {
                    iter.Begin();
                    var list = GetStringsFromIterator(iter);
                    listOfList.Add(list);
                }
            }
        }
        owners = GetOwnerDataFromStrings(listOfList);

        return owners;
    }

    private IList<string> GetStringsFromIterator(ResultIterator iter)
    {
        var list = new List<string>();
        bool hasNext = true;
        do
        {
            list.Add(iter.GetText(PageIteratorLevel.TextLine).Trim().Replace("\n\n", string.Empty));
            hasNext = iter.Next(PageIteratorLevel.TextLine);
        } while (hasNext);
        return list;
    }

    private IEnumerable<string> GetFileNames()
    {
        var directoryPath = _settings.DirectoryPathImage;
        if (string.IsNullOrEmpty(directoryPath))
        {
            return Enumerable.Empty<string>();
        }
        _logger.LogDebug($"path to the directory: {directoryPath}");
        return Directory.EnumerateFiles(directoryPath);
    }
}