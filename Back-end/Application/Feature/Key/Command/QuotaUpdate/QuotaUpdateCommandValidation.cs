
using System;
using FluentValidation;
using Application.Common.Interface;

namespace Application.Feature.Auth.Command.QuotaUpdateCommand;

public class QuotaUpdateCommandValidation : AbstractValidator<QuotaUpdateCommand>
{
    public QuotaUpdateCommandValidation()
    {

        RuleFor(x => x.id)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage(x => string.Format("Id Empty"));

        RuleFor(x => x.Quota)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage(x => string.Format("Quota Empty"))
        .GreaterThan(0).WithMessage(x => string.Format("Quota should be greater than 0"));
    }
}