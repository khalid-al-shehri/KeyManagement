
using MediatR;
using Application.Common.Interface;
using Application.Common.Shared;
using Application.Common.DTO.Auth;
using Application.Common.DTO.Key;
using Domain.Entities;
using Domain.Enum;

namespace Application.Feature.Auth.Command.RevokeKeyCommand;

public record RevokeKeyCommand : IRequest<Result<KeyDto>>
{
    public required Guid id { get; set; }
}

public class RevokeKeyCommandHandler : IRequestHandler<RevokeKeyCommand, Result<KeyDto>>
{
    private readonly IKeyRepo _keyRepo;
    private readonly IKeyServices _keyServices;
    private readonly ILoggedUser _loggedUser;


    public RevokeKeyCommandHandler(IKeyRepo keyRepo, IKeyServices keyServices, ILoggedUser loggedUser)
    {
        _keyRepo = keyRepo;
        _keyServices = keyServices;
        _loggedUser = loggedUser;
    }

    public async Task<Result<KeyDto>> Handle(RevokeKeyCommand request, CancellationToken cancellationToken)
    {

        Key? retrievedKey = await _keyRepo.GetKeyById(request.id, cancellationToken);
        
        if (retrievedKey == null)
            return Result<KeyDto>.Failure($"No key exist!");

        if (retrievedKey.CreatedBy != _loggedUser.Id)
            return Result<KeyDto>.Failure($"Not Authorized to revoke this key");
        
        if (retrievedKey.StatusId == (int)KeyStatusEnum.Revoked)
            return Result<KeyDto>.Failure($"Key already revoked!");

        retrievedKey.RevokedAt = DateOnly.FromDateTime(DateTime.Now);
        retrievedKey.StatusId = (int)KeyStatusEnum.Revoked;

        await _keyRepo.UpdateKey(retrievedKey, cancellationToken);

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
