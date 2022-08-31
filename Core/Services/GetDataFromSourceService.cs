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
    private ObjectPool<StringBuilder> _builderPool;

    public GetDataFromSourceService(ILogger<GetDataFromSourceService> logger, IOptions<Settings> options,
        TesseractEngine engine, ObjectPool<StringBuilder> builderPool)
    {
        _logger = logger;
        _settings = options.Value;
        _engine = engine;
        _builderPool = builderPool;
    }

    public void Dispose() => _engine.Dispose();

    public IEnumerable<OwnerVoting> Get()
    {
        try
        {
            var files = GetFileNames();
            if (!files.Any())
            {
                return Enumerable.Empty<OwnerVoting>();
            }
            var listOfList = GetRawData(files);
            int indexFlatNumber = 0, indexSquareFlat = 0, indexlivingQuater = 0, indexFlatType = 0, indexOwnerName = 0,
                indexSquareOfPart = 0, indexPercentOfTheWholeHouse = 0;
            do
            { }
            while (indexFlatNumber < listOfList[0].Count || indexSquareFlat < listOfList[1].Count
                || indexlivingQuater < listOfList[2].Count || indexFlatType < listOfList[3].Count
                || indexOwnerName < listOfList[4].Count || indexSquareOfPart < listOfList[5].Count
                || indexPercentOfTheWholeHouse < listOfList[6].Count);


        }
        catch (Exception e)
        {
            _logger.LogError(e, "OCR error");
            Console.WriteLine("Unexpected Error: " + e.Message);
            Console.WriteLine("Details: ");
            Console.WriteLine(e.ToString());
        }

        return Enumerable.Empty<OwnerVoting>();
    }

    internal IList<IList<string>> GetRawData(IEnumerable<string> files)
    {
        var listOfList = new List<IList<string>>();
        foreach (var file in files)
        {
            using (var img = Pix.LoadFromFile(file))
            {
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
            }
        }
        return listOfList;
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

    /*
                                    var ownerData = new OwnerData();
                                    var flatNumberString = GetValue(iter).CleanIntString();
                                    if (int.TryParse(flatNumberString, out int flatNumber))
                                    {
                                        ownerData.FlatNumber = flatNumber;
                                    }

                                    var squareFlatString = GetValue(iter).CleanDecimalString();
                                    if (decimal.TryParse(squareFlatString, out decimal squareFlat))
                                    {
                                        ownerData.FlatSquare = squareFlat;
                                    }

                                    var livingQuater = GetValue(iter);
                                    ownerData.LivingQuaterType = livingQuater.GetEnumValueByDisplayName<LivingQuater>();

                                    var flatTypeString = GetValue(iter);
                                    ownerData.TypeOfFlat = flatTypeString.GetEnumValueByDisplayName<FlatType>();

                                    var ownerName = GetValue(iter).CleanString();
                                    ownerData.Name = ownerName;

                                    var squareOfPartString = GetValue(iter).CleanDecimalString();
                                    if (decimal.TryParse(squareOfPartString, out decimal squareOfPart))
                                    {
                                        ownerData.SquareOfPart = squareOfPart;
                                    }

                                    iter.Next(PageIteratorLevel.Block);

                                    var percentOfTheWholeHouseString = GetValue(iter, out hasNext).CleanDecimalString();
                                    if (decimal.TryParse(percentOfTheWholeHouseString, out decimal percentOfTheWholeHouse))
                                    {
                                        ownerData.PercentOfTheWholeHouse = percentOfTheWholeHouse;
                                    }
                                    var vc = new ValidationContext(ownerData);
                                    var errorResults = new List<ValidationResult>();
                                    var isValid = Validator.TryValidateObject(ownerData, vc, errorResults);

                                    if (isValid)
                                    {
                                        owners.Add(ownerData);
                                    }*/

    private string GetValue(ResultIterator iter)
    {
        return GetValue(iter, out _);
    }
    private string GetValue(ResultIterator iter, out bool hasNext)
    {
        var sb = _builderPool.Get();
        var str = $"{iter.GetText(PageIteratorLevel.Block)} ";
        sb.Append(str);
        hasNext = iter.Next(PageIteratorLevel.Block);
        return sb.ToString();
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