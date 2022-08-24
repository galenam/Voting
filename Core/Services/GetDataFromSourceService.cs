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
            var testImagePath = _settings.FilePathTiff;
            _logger.LogDebug($"path to the file: {testImagePath}");

            using (var img = Pix.LoadFromFile(testImagePath))
            {
                using (var page = _engine.Process(img))
                {
                    _logger.LogDebug("Mean confidence: {0}", page.GetMeanConfidence());
                    _logger.LogDebug("Text (iterator):");
                    using (var iter = page.GetIterator())
                    {
                        iter.Begin();
                        var ownerData = new OwnerData();
                        do
                        {
                            var flatNumberString = GetValue(iter);
                            if (int.TryParse(flatNumberString, out int flatNumber))
                            {
                                ownerData.FlatNumber = flatNumber;
                            }

                            var squareFlatString = GetValue(iter);
                            if (decimal.TryParse(squareFlatString, out decimal squareFlat))
                            {
                                ownerData.FlatSquare = squareFlat;
                            }

                            var livingQuater = GetValue(iter);
                            ownerData.LivingQuaterType = livingQuater.GetEnumValueByDisplayName<LivingQuater>();

                            var flatTypeString = GetValue(iter);
                            ownerData.TypeOfFlat = flatTypeString.GetEnumValueByDisplayName<FlatType>();

                            var ownerName = GetValue(iter);
                            ownerData.Name = ownerName;

                            var squareOfPartString = GetValue(iter);
                            if (decimal.TryParse(squareOfPartString, out decimal squareOfPart))
                            {
                                ownerData.SquareOfPart = squareOfPart;
                            }

                            iter.Next(PageIteratorLevel.Block);

                            var percentOfTheWholeHouseString = GetValue(iter);
                            if (decimal.TryParse(percentOfTheWholeHouseString, out decimal percentOfTheWholeHouse))
                            {
                                ownerData.PercentOfTheWholeHouse = percentOfTheWholeHouse;
                            }
                        } while (iter.Next(PageIteratorLevel.Block));
                    }
                }
            }

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
    private string GetValue(ResultIterator iter)
    {
        var sb = _builderPool.Get();
        do
        {
            do
            {
                var str = $"{iter.GetText(PageIteratorLevel.Word)} ";
                sb.Append(str);
            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
        } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));

        iter.Next(PageIteratorLevel.Block);
        return sb.ToString();
    }
}