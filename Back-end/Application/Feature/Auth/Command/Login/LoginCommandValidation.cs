using System;
using FluentValidation;
using Application.Common.Interface;

namespace Application.Feature.Auth.Command.LoginCommand;

public class LoginCommandValidation : AbstractValidator<LoginCommand>
{
    public LoginCommandValidation()
    {

        RuleFor(x => x.Username)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage(x => string.Format("Username Empty"));

        RuleFor(x => x.Password)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage(x => string.Format("Password Empty"));
    }
}
