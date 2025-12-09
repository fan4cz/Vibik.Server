using Api.Application.Common;
using Api.Middlewares;
using Infrastructure.Interfaces;
using Infrastructure.Security;
using Shared.Models.Configs;


var builder = WebApplication.CreateBuilder(args);

builder
    .LoadEnvFiles()
    .AddAuthorizationPolicy()
    .AddAuthServices()
    .AddApplicationServices()
    .ConfigureAppConfigs(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddWeatherServices()
    .AddSwaggerWithAuth()
    .ConfigureWithCertificate();


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