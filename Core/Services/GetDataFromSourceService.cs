using System.Text;
using ExcelDataReader;
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
                                do
                                {
                                    var ownerName = GetValue(iter);

                                    if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                    {
                                        _logger.LogDebug("\n");
                                    }
                                } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
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
            _logger.LogDebug(str);
            sb.Append(str);
        } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

        return sb.ToString();
    }
}