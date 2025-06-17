using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WsCrud.Interfaces;
using WsCrud.Models;
using WsCrud.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swap in-memory repo for file-backed one
builder.Services.AddSingleton<IRepository<Person>>(sp =>
    new JsonPersonRepository("persons.json")); // flat file at app root

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { }