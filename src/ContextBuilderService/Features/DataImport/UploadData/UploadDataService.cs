using System.Text;
using ContextBuilderService.ContextBuilder.UploadData;
using ContextBuilderService.Domain.DataImport;
using Microsoft.SemanticKernel.Text;
using Newtonsoft.Json;
using UglyToad.PdfPig;

namespace ContextBuilderService.Features.DataImport.UploadData;

public interface IUploadDataService
{
    Task<bool> ParseToTxt(IFormFile file);
    Task<bool> GetDataAndChunking(string fileName);
}
public class UploadDataService(IUploadDataRepository repository):IUploadDataService
{
    public async Task<bool> ParseToTxt(IFormFile file)
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


    public async Task<bool> GetDataAndChunking(string fileName)
    {

        var byteArrayTxt=await repository.DownloadBlobData(fileName);
        var fileTxt=Encoding.UTF8.GetString(byteArrayTxt);
        var lines = fileTxt.Split('\n'); 
        var chunks = TextChunker.SplitPlainTextParagraphs(
            lines: lines,
            maxTokensPerParagraph: 200,  
            overlapTokens: 20, 
            chunkHeader: $"DOCUMENT: {fileName}\n\n",
            tokenCounter: null
        );
        var totalChunks = chunks.Count;
        var documentId = Guid.NewGuid(); 
        var chunkModel = chunks.Select((content, index) => new Chunk(
            DocumentId: documentId,
            ChunkIndex: index,
            TotalChunks: totalChunks,
            FileName: fileName,
            Content: content
        )).ToList();
        var jsonChunk=JsonConvert.SerializeObject(chunkModel);
        
        throw new NotImplementedException();
    }
}

