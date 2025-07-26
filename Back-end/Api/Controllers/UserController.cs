using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Common.DTO.Auth;
using Application.Common.Interface;
using Application.Common.Shared;
using Application.Feature.Auth.Command.LoginCommand;
using MediatR;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly ILocalizationService _localizationService;
        private readonly Serilog.ILogger _logger;

        public UsersController(ILocalizationService localizationService, Serilog.ILogger logger)
        {
            _localizationService = localizationService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<Result<TokenDto>> ActivateUser([FromBody] LoginCommand command)
        {
            return await Mediator.Send(command);
        }

    }
}
