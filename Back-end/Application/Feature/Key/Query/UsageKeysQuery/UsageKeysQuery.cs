

using MediatR;
using Application.Common.Interface;
using Application.Common.Shared;
using Application.Common.DTO.Auth;
using Application.Common.DTO.Key;
using Domain.Entities;
using Domain.Enum;
using Application.Common.DTO;
using Application.Common.DTO.KeyUsageDto;

namespace Application.Feature.Auth.Query.UsageKeysQuery;

public record UsageKeysQuery : IRequest<Result<KeyUsageDto>>;

public class UsageKeysQueryHandler : IRequestHandler<UsageKeysQuery, Result<KeyUsageDto>>
{
    private readonly IKeyRepo _keyRepo;


    public UsageKeysQueryHandler(IKeyRepo keyRepo)
    {
        _keyRepo = keyRepo;
    }

    public async Task<Result<KeyUsageDto>> Handle(UsageKeysQuery request, CancellationToken cancellationToken)
    {
        var totalKeysTask = _keyRepo.GetTotalKeys(cancellationToken);
        var keysPerUserTask = _keyRepo.GetKeysPerUser(cancellationToken);
        var totalKeysActiveTask = _keyRepo.GetTotalActive(cancellationToken);
        var totalKeysRevokedTask = _keyRepo.GetTotalRevoked(cancellationToken);
        var topUsedKeysTask = _keyRepo.GetTopUsedKeys(5, cancellationToken);
        var idleKeysTask = _keyRepo.GetIdleKeys(cancellationToken);

        await Task.WhenAll(
            totalKeysTask, 
            keysPerUserTask, 
            totalKeysActiveTask, 
            totalKeysRevokedTask,
            topUsedKeysTask, 
            idleKeysTask
        );

        KeyUsageDto keyUsageDto = new KeyUsageDto
        {
            totalKeys = await totalKeysTask,
            keysPerUser = await keysPerUserTask,
            keysStatus = new KeysStatusDto
            {
                totalActiveKeys = await totalKeysActiveTask,
                totalRevokedKeys = await totalKeysRevokedTask
            },
            topUsedKeys = await topUsedKeysTask,
            idleKeys = await idleKeysTask
        };

        return Result<KeyUsageDto>.Success(keyUsageDto);

    }
}
