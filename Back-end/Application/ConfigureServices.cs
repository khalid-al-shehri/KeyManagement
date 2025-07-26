using FluentValidation;
using MediatR;
using Application.Common.Behavior;
using Application.Common.Interface;
using Application.Common.Service;
using System.Reflection;
using Application.Common.Service.KeyServices;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped<IHashingService, HashingService>();
        services.AddScoped<ILocalizationService, LocalizationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IKeyServices, KeyServices>();

        return services;
    }
}