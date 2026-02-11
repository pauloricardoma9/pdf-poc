using DinkToPdf;
using DinkToPdf.Contracts;

namespace pdf_poc;

public interface IPdfService
{
    Task<byte[]> GeneratePdf(string htmlContent);
}

public class PdfService(IConverter converter, ILogger<PdfService> logger) : IPdfService
{
    private readonly IConverter _converter = converter;
    private readonly ILogger<PdfService> _logger = logger;

    public Task<byte[]> GeneratePdf(string htmlContent)
    {
        try
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Right = 10, Bottom = 10, Left = 10 },
                DocumentTitle = "PDF Document"asdsadas
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
                FooterSettings = { FontSize = 9, Line = false, Center = "PDF gerado via DinkToPdf" }
            };

            var document = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var pdfBytes = _converter.Convert(document);
            return Task.FromResult(pdfBytes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar PDF com DinkToPdf");
            throw;
        }
    }
}