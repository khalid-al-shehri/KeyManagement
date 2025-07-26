using Api.Middleware;
using Application.Common.Shared;
using System.Text.Json.Serialization;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App Setting
AppSettings _appSetting = new AppSettings();
builder.Configuration.GetSection("AppSettings").Bind(_appSetting);
builder.Services.AddSingleton(_appSetting);

// Add Services from different layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, _appSetting);
builder.Services.AddWebApiServices(_appSetting);
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddMemoryCache();

var app = builder.Build();

// app.Urls.Add("http://0.0.0.0:80");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization();

app.UseHttpsRedirection();

app.UseCors();

app.UseMiddleware<RequestLogContextMiddleware>();

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseMiddleware<UserRoleMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();