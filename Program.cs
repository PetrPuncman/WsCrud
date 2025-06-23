using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WsCrud.Interfaces;
using WsCrud.Models;
using WsCrud.Repositories;
using WsCrud.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddSingleton<IFileStorage, FileSystemStorage>();
builder.Services.AddSingleton<IRepository<Person>>(sp =>
    new JsonPersonRepository("persons.json", sp.GetRequiredService<IFileStorage>()));

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { }
