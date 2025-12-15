using System.Reflection;
using Amazon.S3;
using Infrastructure.Api;
using Infrastructure.DataAccess;
using Infrastructure.Interfaces;
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
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            var mediatRConfig = builder.Configuration.GetSection("Licenses").Get<MediatRConfig>();
            cfg.LicenseKey = mediatRConfig.LicenseKey;
            cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
        });

        return builder;
    }
    
    public static WebApplicationBuilder LoadEnvFiles(this WebApplicationBuilder builder)
    {
        DotNetEnv.Env.Load("../.env");
        DotNetEnv.Env.Load();
        builder.Configuration.AddEnvironmentVariables();

        return builder;
    }

    public static WebApplicationBuilder AddAuthorizationPolicy(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("RequireUsername", policy =>
                policy.RequireClaim("username"));

        return builder;
    }

    public static WebApplicationBuilder ConfigureWithCertificate(this WebApplicationBuilder builder)
    {
        var pfxPassword = Environment.GetEnvironmentVariable("PFX_PASSWORD");
        const string pfxPath = "/certs/api.pfx";

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(80);

            if (!string.IsNullOrEmpty(pfxPassword) && File.Exists(pfxPath))
            {
                options.ListenAnyIP(443, listenOptions =>
                {
                    listenOptions.UseHttps(pfxPath, pfxPassword);
                });
            }
        });

        return builder;
    }

    public static WebApplicationBuilder AddAuthServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
        builder.Services.AddSingleton<ITokenService, JwtTokenService>();
        builder.Services.AddAuth(builder.Configuration);
        
        return builder;
    }
    
    public static WebApplicationBuilder  ConfigureAppConfigs(this WebApplicationBuilder builder, IConfiguration config)
    {
        builder.Services.Configure<WeatherConfig>(config.GetSection("Weather"));
        builder.Services.Configure<YosConfig>(config.GetSection("YOS"));
        
        return builder;
    }

    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder, IConfiguration config)
    {
        // S3
        builder.Services.AddSingleton<IAmazonS3>(sp =>
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
        builder.Services.AddScoped<IStorageService, YandexStorageService>();

        // Hasher
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
        
        builder.Services.AddScoped<IUserTable, UserTable>();
        builder.Services.AddScoped<IUsersTasksTable, UsersTasksTable>();
        builder.Services.AddScoped<IMetricsTable, MetricsTable>();
        builder.Services.AddScoped<ITaskEvent, RandomTaskEvent>();
        

        return builder;
    }

    public static WebApplicationBuilder AddWeatherServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient<IWeatherApi, WeatherService>();
        return builder;
    }

    public static WebApplicationBuilder AddSwaggerWithAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
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

        return builder;
    }
}