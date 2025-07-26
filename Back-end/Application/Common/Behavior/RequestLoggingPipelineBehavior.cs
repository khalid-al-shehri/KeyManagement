using MediatR;
using Application.Common.Shared;
using Serilog.Context;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behavior;

public sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : MetaData
{
    private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public RequestLoggingPipelineBehavior(ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        TResponse response = await next();


        if (!response.Succeeded)

            using (LogContext.PushProperty("Error", response.Errors, true))
            {
                _logger.LogError("Completed Request {RequestName} With error", requestName);
            }


        return response;
    }
}

