using System.Text;
using ContextBuilderService.Domain.DataImport;
using Microsoft.SemanticKernel.Text;
using Newtonsoft.Json;

namespace ContextBuilderService.Features.DataImport.GetDataAndChunking;

public interface IGetDataAndChunkingService
{
    Task<string> GetDataAndChunking(string fileName);
}
public class GetDataAndChunkingService(IGetDataAndChunkingRepository repository):IGetDataAndChunkingService
{
    
    public async Task<string> GetDataAndChunking(string fileName)
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
        return jsonChunk;
    }
}