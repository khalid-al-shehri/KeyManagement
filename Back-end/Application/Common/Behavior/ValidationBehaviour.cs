using FluentValidation;
using MediatR;
using System.Reflection;

namespace Application.Common.Behavior;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();


            if (failures.Any())
            {
                var errors = failures.Select(p => p.ErrorMessage).ToArray();

                if (typeof(TResponse).FullName.Contains("Common.Shared.Result"))
                {
                    MethodInfo builderMethod = typeof(TResponse).GetMethod("Failure", BindingFlags.Static | BindingFlags.Public, new Type[] { typeof(IEnumerable<string>) });
                    var res = (TResponse)builderMethod.Invoke(null, new object[] { errors });
                    return res;
                }

            }

        }
        return await next();
    }
}
