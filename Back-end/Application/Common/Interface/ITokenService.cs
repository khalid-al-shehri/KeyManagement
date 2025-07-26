using System;
using System.Security.Claims;
using Domain.Entities;

namespace Application.Common.Interface;

public interface ITokenService
{
    string GenerateToken(User user);
    ClaimsPrincipal? ValidateAndDecodeToken(string token);
    bool IsClaimExist(ClaimsPrincipal principal, string claimType);
    string? GetClaimValue(ClaimsPrincipal principal, string claimType);
}
