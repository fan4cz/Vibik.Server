using Api.Application.Common;
using Api.Middlewares;
using Infrastructure.Interfaces;
using Infrastructure.Security;
using Shared.Models.Configs;


var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load("../.env");
builder.Configuration
    .AddEnvironmentVariables();
var config = builder.Configuration;

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


builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

// Инициализация для подключения к бд
var db = config["POSTGRES_DB"];
var user = config["POSTGRES_USER"];
var password = config["POSTGRES_PASSWORD"];
var host = config["POSTGRES_SERVER"];
var port = config["POSTGRES_PORT"];

var connectionString =
    $"Host={host};Port={port};Database={db};Username={user};Password={password};";

builder.Services.AddScoped<NpgsqlDataSource>(_ => NpgsqlDataSource.Create(connectionString));

//TODO: потом вместо мока поставить реализацю нужную
builder.Services.AddSingleton<IUserTable, UserTableMock>();
builder.Services.AddSingleton<IUsersTasksTable, UsersTasksTableMock>();

// auth
builder.Services.AddSingleton<ITokenService, JwtTokenService>();
builder.Services.AddAuth(builder.Configuration);

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