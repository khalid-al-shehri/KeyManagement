using System;
using FluentValidation;
using Application.Common.Interface;

namespace Application.Feature.Auth.Command.RevokeKeyCommand;

public class RevokeKeyCommandValidation : AbstractValidator<RevokeKeyCommand>
{
    public RevokeKeyCommandValidation()
    {

        RuleFor(x => x.id)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage(x => string.Format("Id Empty"));
    }
}