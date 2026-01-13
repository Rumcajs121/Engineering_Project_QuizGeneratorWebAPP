using BuildingBlocks.CQRS;
using FluentValidation;

namespace ContextBuilderService.Features.DataImport.GetDataAndChunking;

public record GetDataAndChunkingQueryResponse(bool Success);
public record GetDataAndChunkingCommand(string BlobName):ICommand<GetDataAndChunkingQueryResponse>;
public class GetDataAndChunkingValidator:AbstractValidator<GetDataAndChunkingCommand>
{
    //TODO:
}
public class GetDataAndChunkingHandler(IGetDataAndChunkingService service):ICommandHandler<GetDataAndChunkingCommand,GetDataAndChunkingQueryResponse>
{
    public async Task<GetDataAndChunkingQueryResponse> Handle(GetDataAndChunkingCommand command, CancellationToken cancellationToken)
    {
        var result = await service.GetDataAndChunking(command.BlobName);
        return new GetDataAndChunkingQueryResponse(result);
    }
}