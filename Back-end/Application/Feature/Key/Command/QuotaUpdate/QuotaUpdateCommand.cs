
using MediatR;
using Application.Common.Interface;
using Application.Common.Shared;
using Application.Common.DTO.Auth;
using Application.Common.DTO.Key;
using Domain.Entities;
using Domain.Enum;

namespace Application.Feature.Auth.Command.QuotaUpdateCommand;

public record QuotaUpdateCommand : IRequest<Result<KeyDto>>
{
    public required Guid id { get; set; }
    public required int Quota { get; set; }
}

public class QuotaUpdateCommandHandler : IRequestHandler<QuotaUpdateCommand, Result<KeyDto>>
{
    private readonly IKeyRepo _keyRepo;
    private readonly IQuotaService _quotaService;
    private readonly ILoggedUser _loggedUser;


    public QuotaUpdateCommandHandler(IKeyRepo keyRepo, IQuotaService quotaService, ILoggedUser loggedUser)
    {
        _keyRepo = keyRepo;
        _quotaService = quotaService;
        _loggedUser = loggedUser;
    }

    public async Task<Result<KeyDto>> Handle(QuotaUpdateCommand request, CancellationToken cancellationToken)
    {

        Key? retrievedKey = await _keyRepo.GetKeyById(request.id, cancellationToken);
        
        if (retrievedKey == null)
            return Result<KeyDto>.Failure("No key exist!");
        
        if (retrievedKey.CreatedBy != _loggedUser.Id)
            return Result<KeyDto>.Failure("Not Authorized to Update this key");
        
        if (retrievedKey.StatusId == (int)KeyStatusEnum.Revoked)
            return Result<KeyDto>.Failure("Key already revoked!");

        if (retrievedKey.Quota == request.Quota)
            return Result<KeyDto>.Failure("No update made on Quota value");

        retrievedKey.Quota = request.Quota;

        await _keyRepo.UpdateKey(retrievedKey, cancellationToken);

        _quotaService.RemoveKeyQuota(retrievedKey.KeyValue);

        return Result<KeyDto>.Success(new KeyDto
        {
            Id = retrievedKey.Id,
            KeyName = retrievedKey.KeyName,
            KeyValue = retrievedKey.KeyValue,
            Quota = retrievedKey.Quota,
            Status = Enum.GetName(typeof(KeyStatusEnum), retrievedKey.StatusId)!,
            RevokedAt = retrievedKey.RevokedAt,
            CreatedAt = retrievedKey.CreatedAt,
        });

    }
}
