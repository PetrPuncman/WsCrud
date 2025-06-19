using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WsCrud.Interfaces;
using WsCrud.Models;
using WsCrud.Repositories;
using WsCrud.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// DI for persistence
builder.Services.AddSingleton<IFileStorage, FileSystemStorage>();
builder.Services.AddSingleton<IRepository<Person>>(sp =>
    new JsonPersonRepository("persons.json", sp.GetRequiredService<IFileStorage>()));

// Enable Basic Auth
builder.Services.AddAuthentication("BasicAuth")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuth", null);
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }