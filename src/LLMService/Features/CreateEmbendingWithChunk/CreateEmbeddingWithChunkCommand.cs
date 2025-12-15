using System.Windows.Input;
using BuildingBlocks.CQRS;


namespace LLMService.Features.CreateEmbendingWithChunk;

public record CreateEmbeddingWithChunkCommandResponse(bool Success);
public record CreateEmbeddingWithChunkCommand(Guid DocumendId):ICommand<CreateEmbeddingWithChunkCommandResponse>;

public class CreateEmbeddingWithChunkCommandHandler(ICreateEmbendingWithChunkService service):ICommandHandler<CreateEmbeddingWithChunkCommand,CreateEmbeddingWithChunkCommandResponse>
{
    public async Task<CreateEmbeddingWithChunkCommandResponse> Handle(CreateEmbeddingWithChunkCommand request, CancellationToken cancellationToken)
    {
        
        var result = await service.CreateEmbedding(request.DocumendId);
        return new CreateEmbeddingWithChunkCommandResponse(result);
    }
}