using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WsCrud.Interfaces;
using WsCrud.Models;
using WsCrud.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register controllers and repo
builder.Services.AddControllers();
builder.Services.AddSingleton<IRepository<Person>, InMemoryPersonRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { }