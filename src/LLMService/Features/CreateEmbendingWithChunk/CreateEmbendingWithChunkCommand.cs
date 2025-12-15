using System.Windows.Input;
using BuildingBlocks.CQRS;


namespace LLMService.Features.CreateEmbendingWithChunk;

public record CreateEmbendingWithChunkCommandResponse(bool Success);
public record CreateEmbendingWithChunkCommand(Guid DocumendId):ICommand<CreateEmbendingWithChunkCommandResponse>;

public class CreateEmbendingWithChunkCommandHandler(ICreateEmbendingWithChunkService service):ICommandHandler<CreateEmbendingWithChunkCommand,CreateEmbendingWithChunkCommandResponse>
{
    public async Task<CreateEmbendingWithChunkCommandResponse> Handle(CreateEmbendingWithChunkCommand request, CancellationToken cancellationToken)
    {
        
        var result = await service.CreateEmbedding(request.DocumendId);
        return new CreateEmbendingWithChunkCommandResponse(result);
    }
}