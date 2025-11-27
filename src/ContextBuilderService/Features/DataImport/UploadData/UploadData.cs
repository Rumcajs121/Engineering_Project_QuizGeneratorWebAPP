using System.Diagnostics.Contracts;
using BuildingBlocks.CQRS;
using ContextBuilderService.Features.DataImport.UploadData;
using FluentValidation;

namespace ContextBuilderService.ContextBuilder.UploadData;

public record UploadDataCommandResult(bool Success);
public record UploadDataCommand(IFormFile File): ICommand<UploadDataCommandResult>;

public class  UploadDataCommandHandler(IUploadDataService service):ICommandHandler<UploadDataCommand,UploadDataCommandResult>
{
    public async Task<UploadDataCommandResult> Handle(UploadDataCommand request, CancellationToken cancellationToken)
    {
        var result= await service.PdfToTxtUploadService(request.File);
        return new UploadDataCommandResult(result);
    }
}
public class UploadDataValidator:AbstractValidator<UploadDataCommandResult>
{
    
}