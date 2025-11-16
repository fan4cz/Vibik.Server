using System.Reflection;
using Api.Middlewares;
using Infrastructure.Interfaces;
using Infrastructure.Mocks;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Minio;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Подключение Minio с конфигурацией из 
builder.Services.AddSingleton<IMinioClient>(_ => new MinioClient()
    .WithEndpoint(config["Minio:Endpoint"])
    .WithCredentials(config["Minio:AccessKey"], config["Minio:SecretKey"])
    .WithSSL(bool.Parse(config["Minio:WithSsl"]))
    .Build());
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
builder.Services.AddMediatR(config =>
{
    config.LicenseKey = licenseKey;
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();