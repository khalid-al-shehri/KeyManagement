using System;

namespace Application.Common.DTO.Auth;

public record TokenDto
{
    public required string token {get; set;}
}
