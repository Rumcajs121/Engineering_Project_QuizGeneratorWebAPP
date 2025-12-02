using BuildingBlocks.CQRS;
using FluentValidation;

namespace ContextBuilderService.Features.DataImport.GetDataAndChunking;

public record GetDataAndChunkingQueryResponse(bool Success);
public record GetDataAndChunkingQuery(string BlobName):IQuery<GetDataAndChunkingQueryResponse>;
public class GetDataAndChunkingValidator:AbstractValidator<GetDataAndChunkingQuery>
{
    //TODO:
}
public class GetDataAndChunkingHandler(IGetDataAndChunkingService service):IQueryHandler<GetDataAndChunkingQuery,GetDataAndChunkingQueryResponse>
{
    public async Task<GetDataAndChunkingQueryResponse> Handle(GetDataAndChunkingQuery query, CancellationToken cancellationToken)
    {
        var result = await service.GetDataAndChunking(query.BlobName);
        return new GetDataAndChunkingQueryResponse(result);
    }
}