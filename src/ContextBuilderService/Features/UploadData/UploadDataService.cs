using System.Text;
using ContextBuilderService.ContextBuilder.UploadData;
using ContextBuilderService.Domain.Repository;
using ContextBuilderService.Features.DataImport.GetDataAndChunking;
using UglyToad.PdfPig;

namespace ContextBuilderService.Features.DataImport.UploadData;

public interface IUploadDataService
{
    Task<bool> PdfToTxtUploadService(IFormFile file);
}
public class UploadDataService(IRepository repository):IUploadDataService
{
    public async Task<bool> PdfToTxtUploadService(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        using var pdf=PdfDocument.Open(stream);
        var pages = pdf.GetPages()
            .Select(p => p.Text.Trim());

        var text = string.Join("\n\n", pages);
        var txtFileName = Path.ChangeExtension(file.FileName, ".txt");
        var txtBytes = Encoding.UTF8.GetBytes(text);
        using var txtStream = new MemoryStream(txtBytes);
        var txtFormFile = new FormFile(txtStream, 0, txtBytes.Length, txtFileName, txtFileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };
        await repository.UploadDataToBlob(txtFormFile);
        return true;
    }
}

