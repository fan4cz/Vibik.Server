using System.Reflection;
using Amazon.S3;
using Infrastructure.Api;
using Infrastructure.DataAccess;
using Infrastructure.Interfaces;
using Infrastructure.Mocks;
using Infrastructure.Security;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Npgsql;
using Shared.Models.Configs;

namespace Api.Application.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAppConfigs(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<WeatherConfig>(config.GetSection("Weather"));
        services.Configure<YosConfig>(config.GetSection("YOS"));
        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        // S3
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var yosConfig = sp.GetRequiredService<IOptions<YosConfig>>().Value;
            
            if (string.IsNullOrEmpty(yosConfig.Endpoint))
                throw new InvalidOperationException("ENDPOINT не настроен");
            
            var s3Config = new AmazonS3Config
            {
                ServiceURL = yosConfig.Endpoint,
                ForcePathStyle = true
            };
            
            return new AmazonS3Client(
                yosConfig.AccessKey,
                yosConfig.SecretKey,
                s3Config
            );
        });
        services.AddScoped<IStorageService, YandexStorageService>();

        // Hasher
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

        // Инициализация для подключения к бд
        var db = config["POSTGRES_DB"];
        var user = config["POSTGRES_USER"];
        var password = config["POSTGRES_PASSWORD"];
        var host = config["POSTGRES_SERVER"];
        var port = config["POSTGRES_PORT"];

        var connectionString =
            $"Host={host};Port={port};Database={db};Username={user};Password={password};";

        services.AddScoped<NpgsqlDataSource>(_ => NpgsqlDataSource.Create(connectionString));

        //TODO: потом вместо мока поставить реализацю нужную
        services.AddScoped<IUserTable, UserTable>();
        services.AddScoped<IUsersTasksTable, UsersTasksTable>();
        services.AddScoped<IMetricsTable, MetricsTable>();

        return services;
    }

    public static IServiceCollection AddWeatherServices(this IServiceCollection services)
    {
        services.AddHttpClient<IWeatherApi, WeatherService>();
        return services;
    }

    public static IServiceCollection AddSwaggerWithAuth(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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

        return services;
    }
}