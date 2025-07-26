using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Application.Common.Interface;
using Application.Common.Shared;

namespace Infrastructure.Identity;

public class LoggedUser : ILoggedUser
{
    private readonly IHttpContextAccessor _httpContext;

    public LoggedUser(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }


    public Guid Id
    {
        get
        {
            var userId = _httpContext.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "UserId")?.Value;
            return Guid.Parse(userId);
        }
    }

}


