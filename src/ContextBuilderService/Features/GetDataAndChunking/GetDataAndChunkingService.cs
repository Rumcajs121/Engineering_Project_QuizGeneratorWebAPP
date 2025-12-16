using System.Text;
using ContextBuilderService.Domain.DataImport;
using ContextBuilderService.Domain.Repository;
using Microsoft.SemanticKernel.Text;
using Newtonsoft.Json;

namespace ContextBuilderService.Features.DataImport.GetDataAndChunking;

public interface IGetDataAndChunkingService
{
    Task<bool> GetDataAndChunking(string fileName);
}
public class GetDataAndChunkingService(IRepository repository):IGetDataAndChunkingService
{
    
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
        var documentId = Guid.NewGuid(); //TODO: Replace with actual document ID if available 
        var chunkModel = chunks.Select((content, index) => new Chunk(
            DocumentId: documentId,
            ChunkIndex: index,
            TotalChunks: totalChunks,
            FileName: fileName,
            Content: content
        )).ToList();
        await repository.SaveChunkAsync(chunkModel);
        return true;
        
    }
}