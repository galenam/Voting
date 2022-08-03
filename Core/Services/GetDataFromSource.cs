using System.Drawing;
using IronOcr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
namespace Services;
public class GetDataFromSourceService : IGetDataFromSourceService
{
    private ILogger<GetDataFromSourceService> _logger;
    private Settings _settings;
    public GetDataFromSourceService(ILogger<GetDataFromSourceService> logger, IOptions<Settings> options)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public async Task<IEnumerable<OwnerVoting>> Get()
    {
        var ocr = new IronTesseract();
        ocr.Language = OcrLanguage.RussianBest;
        using (var input = new OcrInput())
        {
            try
            {
                foreach (var page in _settings.PagesPdf)
                {
                    var rectangle = new Rectangle(page.StartX, page.StartY, page.Width, page.Height);
                    var cropRectangle = new CropRectangle(rectangle, CropRectangle.MeasurementUnits.Millimeters);
                    input.AddPdfPages($"{Directory.GetCurrentDirectory()}{_settings.FilePathPdf}", page.Numbers.GetPages(), ContentArea: cropRectangle);
                }
                input.EnhanceResolution();
                input.Contrast();
                var result = await ocr.ReadAsync(input);
                _logger.LogInformation(result.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during reading the file");
            }

        }
        return null;
    }
}