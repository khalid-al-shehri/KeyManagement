using MediatR;
using Application.Common.Interface;
using Application.Common.Shared;
using Application.Common.DTO.Auth;
using Application.Common.DTO.Key;
using Domain.Entities;
using Domain.Enum;
using Application.Common.DTO;

namespace Application.Feature.Auth.Query.GetListKeysQuery;

public record GetListKeysQuery : IRequest<Result<PaginationListDTO<List<KeyDto>>>>
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}

public class GetListKeysQueryHandler : IRequestHandler<GetListKeysQuery, Result<PaginationListDTO<List<KeyDto>>>>
{
    private readonly IKeyRepo _keyRepo;
    private readonly IKeyServices _keyServices;
    private readonly ILoggedUser _loggedUser;


    public GetListKeysQueryHandler(IKeyRepo keyRepo, IKeyServices keyServices, ILoggedUser loggedUser)
    {
        _keyRepo = keyRepo;
        _keyServices = keyServices;
        _loggedUser = loggedUser;
    }

    public async Task<Result<PaginationListDTO<List<KeyDto>>>> Handle(GetListKeysQuery request, CancellationToken cancellationToken)
    {
        var listAsDto = await _keyRepo.GetListKeys(request.PageNumber, request.PageSize, cancellationToken);

        return Result<PaginationListDTO<List<KeyDto>>>.Success(listAsDto);

    }
}
