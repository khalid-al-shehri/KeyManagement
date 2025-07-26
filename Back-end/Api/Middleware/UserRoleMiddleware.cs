using System;
using Application.Common.Interface;

namespace Api.Middleware;

public class UserRoleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public UserRoleMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
           var usernameClaim = context.User.Claims.FirstOrDefault(c => c.Type == "Username");
           if (usernameClaim != null)
           {
            //    var username = usernameClaim.Value;
            //    using var serviceScope = _serviceProvider.CreateScope();

            //    var services = serviceScope.ServiceProvider;
            //    var userPermissionsRepo = services.GetRequiredService<IUserPermissionsRepo>();
            //    var userService = services.GetRequiredService<IUserRepo>();

            //    var roles = await userPermissionsRepo.GetUserRolesByUsername(username);

            //    var userId = await userService.GetUserIdByUsername(username);

            //    var claimsIdentity = context.User.Identity as System.Security.Claims.ClaimsIdentity;
               
            //    if(userId != Guid.Empty)
            //         claimsIdentity?.AddClaim(new System.Security.Claims.Claim("UserId", userId.ToString()));



            //    foreach (var role in roles)
            //    {
            //        claimsIdentity?.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role));
            //    }
           }
        }

        await _next(context);
    }
}
