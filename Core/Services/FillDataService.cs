using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Tesseract;

namespace Services;

public class FillDataService : IFillDataService
{
    private readonly ILogger<FillDataService> _logger;
    private readonly Settings _settings;
    private readonly TesseractEngine _engine;

    private readonly IGetDataFromSourceService _dataService;
    public FillDataService(ILogger<FillDataService> logger, IOptions<Settings> options, IGetDataFromSourceService dataService,
        TesseractEngine engine)
    {
        _logger = logger;
        _settings = options.Value;
        _dataService = dataService;
        _engine = engine;
    }

    public async Task<bool> FillDb()
    {
        //var data = await _dataService.Get();
        try
        {
            var testImagePath = _settings.FilePathTiff;

            using (var img = Pix.LoadFromFile(testImagePath))
            {
                using (var page = _engine.Process(img))
                {
                    var text = page.GetText();
                    Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                    Console.WriteLine("Text (GetText): \r\n{0}", text);
                    Console.WriteLine("Text (iterator):");
                    using (var iter = page.GetIterator())
                    {
                        iter.Begin();

                        do
                        {
                            do
                            {
                                do
                                {
                                    do
                                    {
                                        if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                        {
                                            Console.WriteLine("<BLOCK>");
                                        }

                                        Console.Write(iter.GetText(PageIteratorLevel.Word));
                                        Console.Write(" ");

                                        if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                        {
                                            Console.WriteLine();
                                        }
                                    } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                    if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                    {
                                        Console.WriteLine();
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

        return true;
    }
}