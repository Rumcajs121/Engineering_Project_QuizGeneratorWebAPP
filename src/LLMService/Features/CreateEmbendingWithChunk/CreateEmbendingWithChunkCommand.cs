using System.Windows.Input;
using BuildingBlocks.CQRS;

namespace LLMService.Features.CreateEmbendingWithChunk;

public record CreateEmbendingWithChunkCommandResponse(bool Success);
public record CreateEmbendingWithChunkCommand(string DataJson):ICommand<CreateEmbendingWithChunkCommandResponse>;

public class CreateEmbendingWithChunkCommandHandler:ICommandHandler<CreateEmbendingWithChunkCommand,CreateEmbendingWithChunkCommandResponse>
{
    public Task<CreateEmbendingWithChunkCommandResponse> Handle(CreateEmbendingWithChunkCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}