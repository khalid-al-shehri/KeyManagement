
using MediatR;
using Application.Common.Interface;
using Application.Common.Shared;
using Application.Common.DTO.Auth;

namespace Application.Feature.Auth.Command.LoginCommand;

public record LoginCommand : IRequest<Result<TokenDto>>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenDto>>
{
    private readonly IUserRepo _userRepo;
    private readonly ITokenService _tokenService;


    public LoginCommandHandler(IUserRepo userRepo, ITokenService tokenService)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
    }

    public async Task<Result<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {

        // call repository layer
        var user = await _userRepo.GetUserByUsername(request.Username, cancellationToken);

        if (user == null || user.Password != request.Password)
        {
            return Result<TokenDto>.Failure("Username or password not correct, try again");
        }

        var token = _tokenService.GenerateToken(user);

        return Result<TokenDto>.Success(new TokenDto
        {
            token = token
        });

    }
}
