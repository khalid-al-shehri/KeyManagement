using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Application.Common.Interface;
using Application.Common.Shared;

namespace Application.Common.Service;

public class TokenService : ITokenService
{
    private readonly AppSettings _appSetting;

    public TokenService(AppSettings appSetting)
    {
        _appSetting = appSetting;
    }

    public string GenerateToken(Domain.Entities.User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSetting.APISecuritySettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = GenerateClaims(user);
        var token = new JwtSecurityToken(
            issuer: _appSetting.APISecuritySettings.Issuer,
            audience: _appSetting.APISecuritySettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_appSetting.APISecuritySettings.Expiration),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private List<Claim> GenerateClaims(Domain.Entities.User user)
    {
        var claims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim("Username", user.Username.ToString()),
        };

        return claims;
    }

    public ClaimsPrincipal? ValidateAndDecodeToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSetting.APISecuritySettings.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _appSetting.APISecuritySettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _appSetting.APISecuritySettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Remove default 5-minute leniency
            };

            var claims = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            
            return claims;
        }
        catch (SecurityTokenExpiredException ex)
        {
            // Token expired
            return null;
        }
        catch (SecurityTokenInvalidSignatureException ex)
        {
            // Invalid signature
            return null;
        }
        catch (Exception ex)
        {
            // Validation failed
            return null;
        }
    }

    public string? GetClaimValue(ClaimsPrincipal principal, string claimType)
    {
        return principal?.FindFirst(claimType)?.Value;
    }    
    
    public bool IsClaimExist(ClaimsPrincipal principal, string claimType)
    {
        return principal?.FindFirst(claimType)?.Value == null ? false : true;
    }

}

