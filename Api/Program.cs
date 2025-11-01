using System.Reflection;
using Api.Middlewares;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Mocks;
using Infrastructure.Security;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);

// Ключ для MediatR
var licenseKey = builder.Configuration["Licenses:MediatR"];

// документация для Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
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

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();