using BuildingBlocks.CQRS;
using FluentValidation;

namespace ContextBuilderService.Features.DataImport.GetDataAndChunking;

public record GetDataAndChunkingCommandRequest(IFormFile File);
public record GetDataAndChunking:IQuery<GetDataAndChunkingCommandRequest>;
public class GetDataAndChunkingValidator:AbstractValidator<GetDataAndChunking>
{
    //TODO:
}
public class GetDataAndChunkingHandler:IQueryHandler<GetDataAndChunking,GetDataAndChunkingCommandRequest>
{
    public Task<GetDataAndChunkingCommandRequest> Handle(GetDataAndChunking request, CancellationToken cancellationToken)
    {
        
        throw new NotImplementedException();
    }
}