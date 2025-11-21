using System.Reflection;
using Amazon.S3;
using Api.Middlewares;
using Infrastructure.Interfaces;
using Infrastructure.Mocks;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
builder.Configuration
    .AddEnvironmentVariables();
var config = builder.Configuration;

builder.Services.AddSingleton<IAmazonS3>(_ =>
{
    if (string.IsNullOrEmpty(config["YOS_ENDPOINT"]))
        throw new InvalidOperationException("YOS_ENDPOINT не настроен");
    var s3Config = new AmazonS3Config
    {
        ServiceURL = config["YOS_ENDPOINT"],
        ForcePathStyle = true
    };
    return new AmazonS3Client(
        config["YOS_ACCESS_KEY"],
        config["YOS_SECRET_KEY"],
        s3Config
    );
});
//  Настройка Jwt
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<ITokenService, JwtTokenService>();


// Ключ для MediatR
var licenseKey = builder.Configuration["Licenses:MediatR"];

// документация для Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddMediatR(mediartConfig =>
{
    mediartConfig.LicenseKey = licenseKey;
    mediartConfig.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});


builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

// Инициализация для подключения к бд
var connectionString = builder.Configuration.GetConnectionString("DbConnectionString")
                       ?? throw new InvalidOperationException("Connection string missing");
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