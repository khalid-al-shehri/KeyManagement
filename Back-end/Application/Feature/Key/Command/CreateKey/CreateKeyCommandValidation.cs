
using System;
using FluentValidation;
using Application.Common.Interface;

namespace Application.Feature.Auth.Command.CreateKeyCommand;

public class CreateKeyCommandValidation : AbstractValidator<CreateKeyCommand>
{
    public CreateKeyCommandValidation()
    {

        RuleFor(x => x.KeyName)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage(x => string.Format("KeyName Empty"));

        RuleFor(x => x.Quota)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage(x => string.Format("Quota Empty"))
        .GreaterThan(0).WithMessage(x => string.Format("Quota should be greater than 0"));
    }
}