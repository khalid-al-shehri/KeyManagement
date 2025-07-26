
using MediatR;
using Application.Common.Interface;
using Application.Common.Shared;
using Application.Common.DTO.Auth;
using Application.Common.DTO.Key;
using Domain.Entities;
using Domain.Enum;
using Application.Common.DTO;
using Domain.DTO.Key;

namespace Application.Feature.Auth.Command.UseKeyCommand;

public record UseKeyCommand : IRequest<Result<MessageResponseDto>>
{
    public required string KeyValue { get; set; }
}

public class UseKeyCommandHandler : IRequestHandler<UseKeyCommand, Result<MessageResponseDto>>
{
    private readonly IQuotaService _quotaService;


    public UseKeyCommandHandler(IQuotaService quotaService)
    {
        _quotaService = quotaService;
    }

    public async Task<Result<MessageResponseDto>> Handle(UseKeyCommand request, CancellationToken cancellationToken)
    {

        ResultUseKey resultUseKey = await _quotaService.ValidateAndIncrementUsageAsync(request.KeyValue);

        if (!resultUseKey.Succeeded)
            return Result<MessageResponseDto>.Failure(resultUseKey.errorMessage!);
        
        return Result<MessageResponseDto>.Success(new MessageResponseDto
        {
            message = "Key usage increment with 1"
        });

    }
}
