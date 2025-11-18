using DinkToPdf;
using DinkToPdf.Contracts;
using pdf_poc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<IPdfService, PdfService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/pdf", async (IPdfService pdfService) =>
{
    var htmlContent = @"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='UTF-8'>
            <style>
                body { font-family: Arial, sans-serif; margin: 40px; }
                h1 { color: #333; }
                p { line-height: 1.6; }
                .info { background-color: #f0f0f0; padding: 15px; border-radius: 5px; }
            </style>
        </head>
        <body>
            <h1>Documento PDF Gerado com DinkToPdf</h1>
            <p>Este é um exemplo de PDF gerado a partir de HTML usando DinkToPdf.</p>
            <div class='info'>
                <h2>Informações</h2>
                <p><strong>Data:</strong> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</p>
                <p><strong>Tecnologia:</strong> .NET 9.0 + DinkToPdf</p>
                <p><strong>Motor:</strong> wkhtmltopdf</p>
            </div>
        </body>
        </html>
    ";
    
    var pdfBytes = await pdfService.GeneratePdf(htmlContent);
    return Results.File(pdfBytes, "application/pdf", "generated.pdf");
})
.WithName("Gerar Pdf")
.WithDescription("Gera um PDF a partir de conteúdo HTML usando DinkToPdf");

app.Run();