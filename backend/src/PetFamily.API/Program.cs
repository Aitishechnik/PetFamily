using PetFamily.Application.Volonteers;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<IVolonteersRepository, VolonteersRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();
