using Domain.Services;
using Microsoft.Extensions.Options;
using Server.Options;
using TelegramBot.Configuration;

namespace Server.Services;

public class ConvertService : IConverterService
{
    private readonly IOptions<IronOptions> _ironOptions;

    public ConvertService(IOptions<IronOptions> ironOptions)
    {
        _ironOptions = ironOptions;
    }
    
    public Stream ToPdf(string text)
    {
        IronPdf.License.LicenseKey = _ironOptions.Value.Key;
        var renderer = new IronPdf.ChromePdfRenderer();
        var filePdf = renderer.RenderHtmlAsPdf(text);
        filePdf.SaveAs("file.pdf");
        return filePdf.Stream;
    }
}