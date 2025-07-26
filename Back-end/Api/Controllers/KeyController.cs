using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Common.DTO.Auth;
using Application.Common.Interface;
using Application.Common.Shared;
using MediatR;
using Application.Common.DTO.Key;
using Application.Feature.Auth.Command.CreateKeyCommand;
using Application.Common.DTO;
using Application.Feature.Auth.Query.GetListKeysQuery;
using Application.Feature.Auth.Command.RevokeKeyCommand;
using Application.Common.DTO.KeyUsageDto;
using Application.Feature.Auth.Query.UsageKeysQuery;
using Application.Feature.Auth.Command.UseKeyCommand;
using Application.Feature.Auth.Command.VerifyKeyCommand;
using Application.Feature.Auth.Command.QuotaUpdateCommand;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyController : ApiControllerBase
    {

        [HttpPost]
        [Route("create")]
        public async Task<Result<KeyDto>> CreateKey([FromBody] CreateKeyCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        [Route("list")]
        public async Task<Result<PaginationListDTO<List<KeyDto>>>> ListKey([FromQuery] GetListKeysQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPut]
        [Route("revoke")]
        public async Task<Result<KeyDto>> RevokeKey([FromBody] RevokeKeyCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        [Route("usage")]
        public async Task<Result<KeyUsageDto>> UsageKeys()
        {
            UsageKeysQuery query = new UsageKeysQuery();
            return await Mediator.Send(query);
        }

        [HttpPost]
        [Route("verify")]
        public async Task<Result<MessageResponseDto>> VerifyKey([FromBody] VerifyKeyCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        [Route("hello")]
        public async Task<Result<MessageResponseDto>> UseKey([FromBody] UseKeyCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut]
        [Route("quota-update")]
        public async Task<Result<KeyDto>> QuotaUpdate([FromBody] QuotaUpdateCommand command)
        {
            return await Mediator.Send(command);
        }

    }
}
