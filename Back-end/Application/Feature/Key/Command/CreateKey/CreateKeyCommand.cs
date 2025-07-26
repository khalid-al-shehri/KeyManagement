
using MediatR;
using Application.Common.Interface;
using Application.Common.Shared;
using Application.Common.DTO.Auth;
using Application.Common.DTO.Key;
using Domain.Entities;
using Domain.Enum;

namespace Application.Feature.Auth.Command.CreateKeyCommand;

public record CreateKeyCommand : IRequest<Result<KeyDto>>
{
    public required string KeyName { get; set; }
    public required int Quota { get; set; }
}

public class CreateKeyCommandHandler : IRequestHandler<CreateKeyCommand, Result<KeyDto>>
{
    private readonly IKeyRepo _keyRepo;
    private readonly IKeyServices _keyServices;
    private readonly ILoggedUser _loggedUser;


    public CreateKeyCommandHandler(IKeyRepo keyRepo, IKeyServices keyServices, ILoggedUser loggedUser)
    {
        _keyRepo = keyRepo;
        _keyServices = keyServices;
        _loggedUser = loggedUser;
    }

    public async Task<Result<KeyDto>> Handle(CreateKeyCommand request, CancellationToken cancellationToken)
    {

        bool isNameExist = await _keyRepo.IsNameExist(request.KeyName, cancellationToken);
        
        if (isNameExist == true)
            return Result<KeyDto>.Failure("Key's name is already exist");
        
        Key key = new Key
        {
            Id = Guid.NewGuid(),
            KeyName = request.KeyName,
            Quota = request.Quota,
            KeyValue = _keyServices.GenerateKey(),
            StatusId = (int)KeyStatusEnum.Active,
            CreatedBy = _loggedUser.Id
        };

        await _keyRepo.CreateKey(key, cancellationToken);

        return Result<KeyDto>.Success(new KeyDto
        {
            Id = key.Id,
            KeyName = key.KeyName,
            KeyValue = key.KeyValue,
            Quota = key.Quota,
            Status = Enum.GetName(typeof(KeyStatusEnum), key.StatusId)!,
            RevokedAt = key.RevokedAt,
            CreatedAt = key.CreatedAt,
        });

    }
}
