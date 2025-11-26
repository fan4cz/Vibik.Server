using Api.Application.Common;
using Api.Middlewares;
using Infrastructure.Interfaces;
using Infrastructure.Security;
using Shared.Models.Configs;


var builder = WebApplication.CreateBuilder(args);
// DotNetEnv.Env.Load("../.env");
DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureAppConfigs(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

// Weather API
builder.Services.AddWeatherServices();

// auth
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<ITokenService, JwtTokenService>();
builder.Services.AddAuth(builder.Configuration);

// документация для Swagger
builder.Services.AddSwaggerWithAuth();

builder.Services.AddMediatR(cfg =>
{
    var mediatRConfig = builder.Configuration.GetSection("Licenses").Get<MediatRConfig>();
    cfg.LicenseKey = mediatRConfig.LicenseKey;
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});


builder.Services.AddControllers();
var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();