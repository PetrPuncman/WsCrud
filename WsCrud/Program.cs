using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WsCrud.Interfaces;
using WsCrud.Models;
using WsCrud.Repositories;
using WsCrud.Services;

// Create a WebApplicationBuilder — the starting point for configuring services and middleware.
var builder = WebApplication.CreateBuilder(args);

// Register MVC controller services (enables [ApiController]-based endpoints).
builder.Services.AddControllers();

// Dependency injection ... Register dependencies for persistence layer:
// IFileStorage is abstracted as FileSystemStorage (handles file I/O).
builder.Services.AddSingleton<IFileStorage, FileSystemStorage>();

// Dependency injection ... IRepository<Person> is implemented as JsonPersonRepository — data is saved in persons.json.
builder.Services.AddSingleton<IRepository<Person>>(sp =>
		new JsonPersonRepository("persons.json", sp.GetRequiredService<IFileStorage>()));

// Enable Basic Authentication
builder.Services.AddAuthentication("BasicAuth")
		.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuth", null);

// Register authorization services
builder.Services.AddAuthorization();

// Build the configured WebApplication instance.
var app = builder.Build();

// Add authentication middleware to validate requests.
app.UseAuthentication();

// Add authorization middleware to enforce policies/roles.
app.UseAuthorization();

// Map all controller routes.
app.MapControllers();

// Run the web app and start listening for incoming HTTP requests.
app.Run();

// This partial Program class is used by test projects to access internal application context.
public partial class Program { }
