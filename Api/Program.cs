var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly) );
    
var app = builder.Build();
app.MapControllers();
app.Run();    