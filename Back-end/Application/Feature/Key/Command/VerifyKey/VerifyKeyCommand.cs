
using MediatR;
using Application.Common.Interface;
using Application.Common.Shared;
using Application.Common.DTO.Auth;
using Application.Common.DTO.Key;
using Domain.Entities;
using Domain.Enum;
using Application.Common.DTO;
using Domain.DTO.Key;

namespace Application.Feature.Auth.Command.VerifyKeyCommand;

public record VerifyKeyCommand : IRequest<Result<MessageResponseDto>>
{
    public required string KeyValue { get; set; }
}

public class VerifyKeyCommandHandler : IRequestHandler<VerifyKeyCommand, Result<MessageResponseDto>>
{
    private readonly IQuotaService _quotaService;

    public VerifyKeyCommandHandler(IQuotaService quotaService)
    {
        _quotaService = quotaService;
    }

    public async Task<Result<MessageResponseDto>> Handle(VerifyKeyCommand request, CancellationToken cancellationToken)
    {

        ResultUseKey resultUseKey = await _quotaService.IsKeyValid(request.KeyValue);

        if (!resultUseKey.Succeeded)
            return Result<MessageResponseDto>.Failure(resultUseKey.errorMessage!);
        
        return Result<MessageResponseDto>.Success(new MessageResponseDto
        {
            message = "Key is Valid"
        });

    }
}
