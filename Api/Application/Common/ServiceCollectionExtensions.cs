using System.Reflection;
using Amazon.S3;
using Infrastructure.Api;
using Infrastructure.Interfaces;
using Infrastructure.Mocks;
using Infrastructure.Security;
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
        services.Configure<MediatRConfig>(config.GetSection("Licenses"));
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

        // Hasher
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

        // БД инициализация подключения
        var connectionString = config.GetConnectionString("DbConnectionString")
                               ?? throw new InvalidOperationException("Connection string missing");
        services.AddScoped<NpgsqlDataSource>(_ => NpgsqlDataSource.Create(connectionString));

        // БД таблицы
        services.AddSingleton<IUserTable, UserTableMock>();
        services.AddSingleton<IUsersTasksTable, UsersTasksTableMock>();

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