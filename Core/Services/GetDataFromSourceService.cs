using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Models;
using Tesseract;

namespace Services;
public class GetDataFromSourceService : IGetDataFromSourceService
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

    ~GetDataFromSourceService()
    {
        _engine.Dispose();
    }

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

                        do
                        {
                            do
                            {

                                var squareFlatString = GetValue(iter);
                                _logger.LogDebug(squareFlatString);
                                var squareFlat = 0M;
                                decimal.TryParse(squareFlatString, out squareFlat);

                                if (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                {
                                    var livingQuater = GetValue(iter);
                                    _logger.LogDebug(livingQuater);
                                    var lqt = livingQuater.GetEnumValueByDisplayName<LivingQuater>();
                                }

                                if (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                {
                                    var flatTypeString = GetValue(iter);
                                    _logger.LogDebug(flatTypeString);
                                    var flatType = flatTypeString.GetEnumValueByDisplayName<FlatType>();
                                }

                                if (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                {
                                    var ownerName = GetValue(iter);
                                    _logger.LogDebug(ownerName);
                                }
                                
                                if (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                {
                                    var squareOfPartString = GetValue(iter);
                                    _logger.LogDebug(squareOfPartString);
                                    var squareOfPart = 0M;
                                    decimal.TryParse(squareOfPartString, out squareOfPart);
                                }
                                
                                iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine);
                                
                                if (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                {
                                    var percentOfTheWholeHouseString = GetValue(iter);
                                    _logger.LogDebug(percentOfTheWholeHouseString);
                                    var percentOfTheWholeHouse = 0M;
                                    decimal.TryParse(percentOfTheWholeHouseString, out percentOfTheWholeHouse);
                                }

                            } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
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
            var str = $"{iter.GetText(PageIteratorLevel.Word)} ";
            sb.Append(str);
        } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

        return sb.ToString();
    }
}