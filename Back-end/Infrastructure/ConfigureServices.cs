using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Application.Common.Interface;
using Application.Common.Shared;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Service;
using Refit;
using Serilog;
using Domain.Repository;
using Application.Common.Service;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, AppSettings appSetting)
    {
        services.AddDbContextFactory<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
                               
        var redisConnection = configuration.GetConnectionString("RedisConnection");
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redisConnection)
        );
        
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddTransient<IStringServices, StringServices>();
        services.AddScoped<ILoggedUser, LoggedUser>();
        services.AddScoped<IQuotaService, QuotaService>();

        services.AddSingleton(Log.Logger);

        // Repositories 
        services.AddScoped<IUserRepo, UserRepo>();
        services.AddScoped<IKeyRepo, KeyRepo>();

        return services;
    }
}

