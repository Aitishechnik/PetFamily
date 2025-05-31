using PetFamily.Application;
using PetFamily.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddInfrastructure()
    .AddApplication();

var app = builder.Build();

app.MapControllers();

app.Run();
